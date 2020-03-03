# Как завести k8 локально и не очень

### 1. Дать доступ k8 к private repositories

#### 1.1 Нормальный способ

1. Получить файл с creds
Берем файл из ~/.docker/config.json
Этот файл получается, когда выполняем docker login.
Можно взять за образец и добавить туда что угодно.<br/><br/>
!!! Проблемы под Windows (и под другими ОС, скорее всего тоже).<br/>
Когда делаешь docker login, то docker сохраняет креды в creadsStore, к которому k8, разумеется доступа не имеет,
и т.о. файл в таком виде передавать в k8 нет смысла.
Чтобы docker сохранял креды просто в файл, нужно в файле config.json проставить creadsStore="".
Тогда docker при login будет сохранять креды в файл и можно будет его передавать в k8. 

2. Передаем файл с кредами в k8, как секрет
    ```
    kubectl create secret generic regcred \
        --from-file=.dockerconfigjson=/C:/Users/kir/.docker/config.json \
        --type=kubernetes.io/dockerconfigjson
    ```
3. Передаем секрет в каждый pod (где надо)
     ```
    containers:
      - name: backend
    ...
    imagePullSecrets:
      - name: regcred
    ```

#### 1.2 Работа с kind

*kind* пока не может рабоать c docker.pkg.github.com потому, что
[проблема с containerd](https://github.com/containerd/containerd/issues/3291).

1. Загрузить нужный image в docker локально. \
`docker pull docker.pkg.github.com/foo/bar/my-image:v1`
2. Загрузить свежезагруженный image в кластер kind. \
`kind load docker-image docker.pkg.github.com/foo/bar/my-image:v1 --name my-claster`
3. Если используется *latest* версия image, то k8 будет при каждом развертывании пытаться
вытащить ее из оригинального репозитория (что логично и желаемо). Но в данном случае, нам
нужно запретить k8 ходить в оригинальный репозиторий.
    ```
    ...
    containers:
      - name: my-container
        image: docker.pkg.github.com/foo/bar/my-image:latest
        imagePullPolicy: IfNotPresent # IfNotPresent or Never
    ...
    ```

f0cf0db867e498518e6f10d27193b4285232df1c


Нормально аутентифицироваться в GitHub packeges средствами k8 в kind не получается


https://github.com/containerd/containerd/issues/3291