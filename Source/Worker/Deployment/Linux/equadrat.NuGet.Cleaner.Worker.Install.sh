#!/bin/bash

# This is an adaptation of the vsts.agent svc.sh script (Azure DevOps Agent / VSTS-Agent).

SVC_NAME=`systemd-escape --path "equadrat.NuGet.Cleaner.Worker.service"`
SVC_DESCRIPTION="equadrat NuGet Cleaner (Worker)"

SVC_CMD=$1
arg_2=${2}
arg_3=${3}
arg_4=${4}

SVC_ROOT=`pwd`

UNIT_PATH=/etc/systemd/system/${SVC_NAME}
TEMPLATE_PATH=./equadrat.NuGet.Cleaner.Worker.service.template
TEMP_PATH=./equadrat.NuGet.Cleaner.Worker.service.temp
CONFIG_PATH=.service
EXECUTABLE_FILE=equadrat.NuGet.Cleaner.Worker
ENVIRONMENT=${arg_3:-Production}
WORK_DIR=${arg_4:-${SVC_ROOT}}

user_id=`id -u`

# systemctl must run as sudo
# this script is a convenience wrapper around systemctl
if [ $user_id -ne 0 ]; then
    echo "Must run as sudo"
    exit 1
fi

function failed()
{
   local error=${1:-Undefined error}
   echo "Failed: $error" >&2
   exit 1
}

if [ ! -f "${TEMPLATE_PATH}" ]; then
    failed "Must run from agent root or install is corrupt"
fi

#check if we run as root
if [[ $(id -u) != "0" ]]; then
    echo "Failed: This script requires to run with sudo." >&2
    exit 1
fi

function install()
{
    echo "Creating launch agent in ${UNIT_PATH}"
    if [ -f "${UNIT_PATH}" ]; then
        failed "error: exists ${UNIT_PATH}"
    fi

    if [ -f "${TEMP_PATH}" ]; then
      rm "${TEMP_PATH}" || failed "failed to delete ${TEMP_PATH}"
    fi

    # can optionally use username supplied
    run_as_user=${arg_2:-$SUDO_USER}
    echo "Run as user: ${run_as_user}"

    run_as_uid=$(id -u ${run_as_user}) || failed "User does not exist"
    echo "Run as uid: ${run_as_uid}"

    run_as_gid=$(id -g ${run_as_user}) || failed "Group not available"
    echo "gid: ${run_as_gid}"

    # create work-dir
    if [ ! -d "${WORK_DIR}" ]; then
        mkdir "${WORK_DIR}"

        if [ -f "./appsettings.${ENVIRONMENT}.json" ]; then
            cp ./appsettings.${ENVIRONMENT}.json -t "${WORK_DIR}"
        fi

        cp ./appsettings.json -t "${WORK_DIR}"

        chown -R ${run_as_uid}:${run_as_gid} "${WORK_DIR}" || failed "failed to set owner for ${WORK_DIR}"
        chmod -R 644 "${WORK_DIR}" || failed "failed to set permissions for ${WORK_DIR}"
        chmod 755 "${WORK_DIR}" || failed "failed to set permissions for ${WORK_DIR}"
    fi

    # create systemd service file
    sed "s/{{User}}/${run_as_user}/g; s/{{Description}}/$(echo ${SVC_DESCRIPTION} | sed -e 's/[\/&]/\\&/g')/g; s/{{ServiceRoot}}/$(echo ${SVC_ROOT} | sed -e 's/[\/&]/\\&/g')/g; s/{{ExecutableFile}}/$(echo ${EXECUTABLE_FILE} | sed -e 's/[\/&]/\\&/g')/g; s/{{WorkDir}}/$(echo ${WORK_DIR} | sed -e 's/[\/&]/\\&/g')/g; s/{{Environment}}/$(echo ${ENVIRONMENT} | sed -e 's/[\/&]/\\&/g')/g;" "${TEMPLATE_PATH}" > "${TEMP_PATH}" || failed "failed to create replacement temp file"
    mv "${TEMP_PATH}" "${UNIT_PATH}" || failed "failed to copy unit file"
    
    # unit file should not be executable and world writable
    chmod 664 "${UNIT_PATH}" || failed "failed to set permissions on ${UNIT_PATH}"
    
    command -v sestatus && sestatus | grep "SELinux status: *enabled"
    is_selinux_enabled=$?
    if [ $is_selinux_enabled -eq 0 ]; then
        # SELinux is enabled, we must ensure the system context for the unit file matches the expected systemd_unit_file context.
        chcon system_u:object_r:systemd_unit_file_t:s0 "${UNIT_PATH}"
    fi
    systemctl daemon-reload || failed "failed to reload daemons"
    
    # Since we started with sudo, EXECUTABLE_FILE will be owned by root. Change this to current login user.    
    chown ${run_as_uid}:${run_as_gid} ./${EXECUTABLE_FILE} || failed "failed to set owner for ${EXECUTABLE_FILE}"
    chmod 755 ./${EXECUTABLE_FILE} || failed "failed to set permission for ${EXECUTABLE_FILE}"
    if [ $is_selinux_enabled -eq 0 ]; then
        # SELinux is enabled, we must ensure the shell scripts matches the expected context.
        chcon system_u:object_r:usr_t:s0 ${EXECUTABLE_FILE}
    fi

    systemctl enable ${SVC_NAME} || failed "failed to enable ${SVC_NAME}"

    echo "${SVC_NAME}" > ${CONFIG_PATH} || failed "failed to create ${CONFIG_PATH} file"
    chown ${run_as_uid}:${run_as_gid} ${CONFIG_PATH} || failed "failed to set permission for ${CONFIG_PATH}"
}

function start()
{
    systemctl start ${SVC_NAME} || failed "failed to start ${SVC_NAME}"
    status    
}

function stop()
{
    systemctl stop ${SVC_NAME} || failed "failed to stop ${SVC_NAME}"    
    status
}

function uninstall()
{
    stop

    if [ -f "${UNIT_PATH}" ]; then
        systemctl disable ${SVC_NAME} || failed "failed to disable  ${SVC_NAME}"
        rm "${UNIT_PATH}" || failed "failed to delete ${UNIT_PATH}"
    fi

    if [ -f "${CONFIG_PATH}" ]; then
        rm "${CONFIG_PATH}" || failed "failed to delete ${CONFIG_PATH}"
    fi

    systemctl daemon-reload || failed "failed to reload daemons"
}

function status()
{
    if [ -f "${UNIT_PATH}" ]; then
        echo
        echo "${UNIT_PATH}"
    else
        echo
        echo "not installed"
        echo
        return
    fi

    systemctl --no-pager status ${SVC_NAME}
}

function usage()
{
    echo
    echo Usage:
    echo "./equadrat.NuGet.Cleaner.Worker.Install.sh [install, start, stop, status, uninstall]"
    echo "Commands:"
    echo "   install service-user environment work-dir: Install the service."
    echo "   start: Start the service."
    echo "   stop: Stop the service."
    echo "   status: Display status of service."
    echo "   uninstall: Uninstall service."
    echo
}

case $SVC_CMD in
   "install") install;;
   "status") status;;
   "uninstall") uninstall;;
   "start") start;;
   "stop") stop;;
   "status") status;;
   *) usage;;
esac

exit 0
