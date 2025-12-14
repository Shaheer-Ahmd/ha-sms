# MSSQL Server on Kubernetes with High Availability

This guide provides step-by-step instructions for deploying Microsoft SQL Server on Kubernetes with persistent storage for high availability.

## Overview

This setup deploys SQL Server 2022 on Kubernetes using:
- **Persistent Volume (PV)** and **Persistent Volume Claim (PVC)** for data persistence
- **Deployment** with init containers for proper permissions
- **NodePort Service** for external access
- **Kubernetes Secrets** for secure password management

## Prerequisites

### Required Software

1. **Docker Desktop** with Kubernetes enabled
   - Download: https://www.docker.com/products/docker-desktop
   - Enable Kubernetes: Settings → Kubernetes → Enable Kubernetes

2. **kubectl** (Kubernetes CLI)
   - Comes with Docker Desktop, or install separately:
   ```powershell
   winget install Kubernetes.kubectl
   ```

3. **kind** (Kubernetes IN Docker) - Optional, alternative to Docker Desktop K8s
   ```powershell
   winget install Kubernetes.kind
   ```

4. **sqlcmd** (SQL Server command-line tool)
   ```powershell
   winget install Microsoft.SqlServer.SqlCmd
   ```

5. **Azure Data Studio** (Optional, GUI for SQL Server)
   - Download: https://learn.microsoft.com/en-us/azure-data-studio/download-azure-data-studio

### System Requirements

- **RAM**: Minimum 6-8 GB allocated to Docker Desktop
  - Docker Desktop → Settings → Resources → Memory → Set to 6-8 GB
- **Disk Space**: At least 10 GB free
- **CPU**: 2+ cores recommended

## Project Structure

```
k8s/
├── mssql-deployment.yaml    # SQL Server deployment configuration
├── mssql-sc-pv-pvc.yaml     # Storage Class, Persistent Volume, and PVC
├── mssql-secret.yaml        # Secret for SA password
├── mssql-service.yaml       # Kubernetes service for external access
└── README.md                # This file
```

## Configuration Files

### 1. mssql-secret.yaml

Stores the SQL Server SA password securely.

```yaml
apiVersion: v1
kind: Secret
metadata:
  name: mssql-secret
type: Opaque
stringData:
  SA_PASSWORD: "YourStrong!Passw0rd"
```

> **Note**: Password must meet SQL Server complexity requirements:
> - Minimum 8 characters
> - Include uppercase, lowercase, numbers, and special characters

### 2. mssql-sc-pv-pvc.yaml

Defines persistent storage for SQL Server data.

```yaml
apiVersion: v1
kind: PersistentVolume
metadata:
  name: mssql-pv
spec:
  storageClassName: ""
  capacity:
    storage: 2Gi
  accessModes:
    - ReadWriteOnce
  persistentVolumeReclaimPolicy: Retain
  hostPath:
    path: "/run/desktop/mnt/host/c/k8s-data/mssql"
    type: DirectoryOrCreate

---
apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: mssql-pvc
spec:
  storageClassName: ""
  accessModes:
    - ReadWriteOnce
  resources:
    requests:
      storage: 2Gi
  volumeName: mssql-pv
```

> **Path Notes**:
> - Windows with Docker Desktop: `/run/desktop/mnt/host/c/k8s-data/mssql` maps to `C:\k8s-data\mssql`
> - Mac: `/Users/yourusername/k8s-data/mssql`
> - Linux: `/home/yourusername/k8s-data/mssql`

### 3. mssql-deployment.yaml

Deploys SQL Server with proper permissions and resource limits.

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: mssql-deployment
spec:
  replicas: 1
  selector:
    matchLabels:
      app: mssql
  template:
    metadata:
      labels:
        app: mssql
    spec:
      terminationGracePeriodSeconds: 30
      hostname: mssqlinst
      securityContext:
        runAsUser: 0
        fsGroup: 0
      initContainers:
      - name: init-mssql
        image: mcr.microsoft.com/mssql/server:2022-latest
        command: ["/bin/sh", "-c"]
        args:
          - |
            mkdir -p /var/opt/mssql/data /var/opt/mssql/log
            chown -R 0:0 /var/opt/mssql
            chmod -R 755 /var/opt/mssql
        volumeMounts:
        - name: mssqldb
          mountPath: /var/opt/mssql
      containers:
      - name: mssql
        image: mcr.microsoft.com/mssql/server:2022-latest
        resources:
          requests:
            memory: "4G"
            cpu: "2000m"
          limits:
            memory: "4G"
            cpu: "2000m"
        ports:
        - containerPort: 1433
        env:
        - name: MSSQL_PID
          value: "Developer"
        - name: ACCEPT_EULA
          value: "Y"
        - name: MSSQL_DATA_DIR
          value: "/var/opt/mssql/data"
        - name: MSSQL_LOG_DIR
          value: "/var/opt/mssql/log"
        - name: SA_PASSWORD
          valueFrom:
            secretKeyRef:
              name: mssql-secret
              key: SA_PASSWORD
        volumeMounts:
        - name: mssqldb
          mountPath: /var/opt/mssql
        livenessProbe:
          tcpSocket:
            port: 1433
          initialDelaySeconds: 60
          periodSeconds: 10
        readinessProbe:
          tcpSocket:
            port: 1433
          initialDelaySeconds: 30
          periodSeconds: 10
      volumes:
      - name: mssqldb
        persistentVolumeClaim:
          claimName: mssql-pvc
```

### 4. mssql-service.yaml

Exposes SQL Server externally via NodePort.

```yaml
apiVersion: v1
kind: Service
metadata:
  name: mssql-service
spec:
  type: NodePort
  selector:
    app: mssql
  ports:
    - protocol: TCP
      port: 1433
      targetPort: 1433
      nodePort: 31433
```

## Deployment Steps

### Step 1: Create the kind cluster (if using kind)

```bash
kind create cluster --name mssql-cluster
```

### Step 2: Apply Kubernetes configurations

Run these commands in the k8s folder order:

```bash
# Create the secret
kubectl apply -f mssql-secret.yaml

# Create persistent volume and claim
kubectl apply -f mssql-sc-pv-pvc.yaml

# Create the deployment
kubectl apply -f mssql-deployment.yaml

# Create the service
kubectl apply -f mssql-service.yaml
```

### Step 3: Verify deployment

```bash
# Check all resources
kubectl get all

# Check if PV and PVC are bound
kubectl get pv
kubectl get pvc

# Watch pod status
kubectl get pods -w
```

Wait until the pod shows `Running` with `1/1` Ready.

## Connecting to SQL Server

### Port Forward (Recommended for kind)

```bash
kubectl port-forward svc/mssql-service 1433:1433
```

Keep this terminal open, then connect in another terminal to make the StudentManagementDB:

```powershell
sqlcmd -S localhost,1433 -U sa -P YourStrong!Passw0rd -C -d master -i ".\StudentManagementDB_creation_new.sql"
```

## Testing High Availability

### Step 1: Simulate pod failure

```bash
kubectl delete pod -l app=mssql
```

### Step 2: Wait for pod restart

```bash
kubectl get pods -w
```

### Step : Port Forward
Once the new pod is running run the port forward again:

```bash
kubectl port-forward svc/mssql-service 1433:1433
```

### Step 4: Verify data persistence
Reconnect and check data:

If you see your data, **persistent storage is working correctly!**

## Useful Commands

### Check pod status
```bash
kubectl get pods
kubectl describe pod <pod-name>
```

### View logs
```bash
kubectl logs <pod-name>
kubectl logs -f <pod-name>  # Follow logs
```

### Execute commands in pod
```bash
kubectl exec -it <pod-name> -- /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -C
```

### Check databases
```powershell
sqlcmd -S localhost,1433 -U sa -P "YourStrong!Passw0rd" -C -Q "SELECT name FROM sys.databases"
```

### Count table rows
```powershell
sqlcmd -S localhost,1433 -U sa -P "YourStrong!Passw0rd" -C -d DatabaseName -Q "SELECT COUNT(*) FROM TableName"
```

### Delete and recreate everything
```bash
kubectl delete deployment mssql-deployment
kubectl delete svc mssql-service
kubectl delete pvc mssql-pvc
kubectl delete pv mssql-pv
kubectl delete secret mssql-secret

# If PVC/PV stuck, force delete:
kubectl patch pvc mssql-pvc -p '{"metadata":{"finalizers":null}}'
kubectl patch pv mssql-pv -p '{"metadata":{"finalizers":null}}'
```

## Troubleshooting

### Pod stuck in ContainerCreating
- Check PVC binding: `kubectl get pvc`
- Check events: `kubectl describe pod <pod-name>`

### Pod in CrashLoopBackOff
- Check logs: `kubectl logs <pod-name> --previous`
- Common causes:
  - Password doesn't meet complexity requirements
  - Permission issues on volume
  - Insufficient memory

### Permission denied errors
Ensure the init container is setting permissions:
```yaml
initContainers:
- name: init-mssql
  command: ["/bin/sh", "-c"]
  args:
    - |
      mkdir -p /var/opt/mssql/data /var/opt/mssql/log
      chown -R 0:0 /var/opt/mssql
      chmod -R 755 /var/opt/mssql
```

### Memory errors (Error 802, Error 17300)
Increase memory in deployment:
```yaml
resources:
  requests:
    memory: "4G"
  limits:
    memory: "4G"
```

Also ensure Docker Desktop has enough memory allocated (6-8 GB minimum).

### Connection refused
1. Check pod is running: `kubectl get pods`
2. Check port-forward is active
3. Try using `127.0.0.1` instead of `localhost`
4. Try different port: `kubectl port-forward svc/mssql-service 11433:1433`

## License

This project uses SQL Server Developer Edition, which is free for development and testing purposes only. For production use, appropriate SQL Server licensing is required.