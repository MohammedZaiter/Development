# Introduction

This is the **Flowon Kubernetes Operator** (Ansible based), with the following roles:

- **FlowonSLA** SLA for FlowonRuntime
- **FlowonDeployment** Base Resources and parent namespace for FlowonRuntime
- **FlowonRuntime** install and configure a Flowon Application
- **FlowonServer** install and configure a Flowon Server (API)

Along with a couple of utilities that help the CI/CD pipeline for flowon development and documentation.

<br>

# Vagrant

Vagrant enables building a virtual machine.

## Prerequisites ( Windows )

We need "make" on Windows, if make is not available you can install GitBash or Choco

Choco: Install chocolatey (choco) using powershell as administrator

```
Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
```

```
choco install make
```

You can also install Vagrant using Choco or using the installer from Vagrant website ( https://www.vagrantup.com/downloads )

```
choco install vagrant
```

If you face errors during launching vagrant, you need to enable hyper v features

```
Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All -Verbose
Enable-WindowsOptionalFeature -Online -FeatureName Containers -All -Verbose
bcdedit /set hypervisorlaunchtype Auto
```

# Installing Operator SDK

It is recommended - for operator development - to use a linux machine, and follow the instructions in the following link to be able to build, run and test the operator locally on the machine.

https://sdk.operatorframework.io/docs/building-operators/ansible/

<br>

# Running, Building and Deploying the Operator

- `source ~/.python-venv/ansible/bin/activate`
- `make deploy` will deploy the required CRD, RBAC to configured cluster using kubectl)
- `make install run` will run the operator locally (make sure to install requirements ansible 2.5+, python 3.9+, openshift, ansible runner, ansible runner http)

**Note**: This will deploy the operator on the kubernetes cluster configured to run in the current context for kubectl.

## Building operator docker image, Installing the operator via docker image

\# `operatorbuild=$(date '+%Y%m%d%H%M%S')` \
\# `make docker-build IMG=index.docker.io/netohm/flowon-operator:latest` \
\# `make docker-push IMG=index.docker.io/netohm/flowon-operator:latest` \
\# `make deploy IMG=index.docker.io/netohm/flowon-operator:latest`

<br>

# Operator Roles

## Flowon Role

The flowon app role installs and updates _FlowonRuntime_(s). For each _FlowonRuntime_, it will create a namespace with the following:

- Secrets
- Ingress
- Config Maps
- Services
  - api-service
  - notification-service
  - media-service
  - supervisor
  - worker
- Deployments (replicasets and related pods and initializer pods) for each service

<br>

## FlowonServer Role

The Flowon server roles installs and updates a flowon server and - if necessary - a mssql server pod to handle data if no connection string is granted.

The "roles/flowonserver/tasks/main.yml" file contains the definition of the application in terms of kubernetes objects and their state.

The "roles/flowonserver/templates" folder contains the j2 template definitions in terms of the parameters passed in the yaml/json file upon the _FlowonServer_ resource creation.

It is important to note that the secret to pull the flowon-server image(s) relies on the "Azure Registry Secret", defined in the file **flowon_server.yaml.j2**.

### Sample FlowonServer CRD (yaml)

<pre>
ingress:
  url: dev-server.flowon.com
  tls:
    crt: base64=====
    key: base64=====
namespace: flowon
database:
  connection.string: ""
access:
  admin:
    login: netways
    pass: somepass
</pre>

<br>

# Additional Files

## imago-deployment.yaml

The file imago-deployment.yaml is used to deploy an updater (see https://github.com/philpep/imago) for the flowon services pods when images are updated on the docker registry.
Mainly useful for CI/CD situations.
The imago CronJob and pods will be run in namespace **flowon-utilities**

## flowon-wiki.yaml

The file flowon-wiki.yaml depolys the flowon documentation (WikiJs, see https://js.wiki/) on namespace **flowon-wiki** (must be created prior).

<br>

# debug / troubleshooting / patching commands

kubectl get all ns
kubectl get all -n flowon-operator-system
kubectl get FlowonRuntime
kubectl logs pod/flowon-operator-xxxxxx --tail=100 -f

kubectl patch crd/flowonruntimes.apps.flowon.com -p '{"metadata":{"finalizers":[]}}' --type=merge

kubectl patch FlowonRuntime/flowonappdev -p '{ "spec": { "services": { "media": { "installed": false, "minPodCount": 0, "appSettings": "" }, "search": { "installed": false, "minPodCount": 0, "appSettings": "" }, "notifications": { "installed": false, "minPodCount": 0, "appSettings": "" }, "activities": { "installed": false, "minPodCount": 0, "appSettings": "" } }}}' --type=merge
