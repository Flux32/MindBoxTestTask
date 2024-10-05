# Задание №1
Напишите на C# библиотеку для поставки внешним клиентам, которая умеет вычислять площадь круга по радиусу и треугольника по трем сторонам. Дополнительно к работоспособности оценим:

- Юнит-тесты
- Легкость добавления других фигур
- Вычисление площади фигуры без знания типа фигуры в compile-time
- Проверку на то, является ли треугольник прямоугольным

## Решение:
[Библиотека](https://github.com/Flux32/MindBoxTestTask/tree/main/MindBoxGeometryLibrary)  
[Юнит тесты](https://github.com/Flux32/MindBoxTestTask/tree/main/MindBoxGeometryLibrary.Tests)

# Задание №2
Опишисать решение для веб-приложения в kubernetes в виде yaml-манифеста. Оставить в коде комментарии по принятым решениям. 

Есть следующие вводные:
У нас kubernetes кластер, в котором пять нод.
Приложение испытывает постоянную стабильную нагрузку в течение суток без значительных колебаний. 3 пода справляются с нагрузкой.
На первые запросы приложению требуется значительно больше ресурсов CPU, в дальнейшем потребление ровное в районе 0.1 CPU. По памяти всегда “ровно” в районе 128M memory.
Приложение требует около 5-10 секунд для инициализации.

Что хотим?

Минимальное потребление ресурсов от этого deployment’а.
Размещение подов на разных нодах для отказоустойчивости.
Чтобы под не обрабатывал запросы до завершения инициализации.

## Решение

Конфигурация Deployment
```yml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: mindbox-k8s-test-deployment

spec:
  replicas: 3 # так как 3 пода справляются с нагрузкой 

selector:
  matchLabels:
    app: mindbox-k8s-test

template: # создаем шаблон для создания подов
  metadata:
    labels: # добавляем к каждому поду метки для их идентификации с приложением
      app: mindbox-k8s-test
  spec:
    containers:
    - name: mindbox-k8s-test-container # задаем имя контейнеру
      image: mindbox-k8s-test-image:latest # запускаем всегда последний образ 
      
      ports:
      - containerPort: 80

      resources:
        requests: # выделяем минимальное кол-во ресурсов
          memory: "128Mi" # так как среднее потребление в районе 128Mi
          cpu: "0.1" # Аналогично и для CPU
        limits: # устанавливаем максимальное кол-во ресурсов
          memory: "256Mi" 
          cpu: "1"

      readinessProbe: # проверяем готовность контейнера к обработке запросов
        httpGet: # устанавливаем, что проверка будет осуществляться через HTTP
          path: /
          port: 80
        initialDelaySeconds: 20 # задержка перед проверкой после запуска контейнера
        periodSeconds: 10 # интервал между проверками

      livenessProbe: # проверка живой ли контейнер
        httpGet: # устанавливаем, что проверка будет осуществляться через HTTP
          path: /
          port: 80
        initialDelaySeconds: 15 # аналогично добавляем задержку после запуска
        periodSeconds: 10 # интервал между проверками
    
    affinity: # задаем правила распределения подов между нодами
        podAntiAffinity: # запрещаем размещать несколько подов на одной ноде, размещаем их на разных для отказоустойчивости
          requiredDuringSchedulingIgnoredDuringExecution: # строгое правило
          - labelSelector:
              matchExpressions:
              - key: app
                operator: In
                values:
                - mindbox-k8s-test
            topologyKey: "kubernetes.io/hostname"
```

Конфигурация точки доступа
```yml
apiVersion: v1
kind: Service
metadata:
  name: mindbox-k8s-test-service
spec:
  selector:
    app: mindbox-k8s-test # обслуживаем поды с этой меткой
  ports:
  - protocol: TCP
    port: 80
    targetPort: 80
  type: ClusterIP
```
