# Introduction.

The objective of that application is to be able to create a secret within kubernetes and an ingress.  has an endpoint that receives a certificate and it will have to register that certificate with a secret within kubernetes and in the ingress it will register a DNS.

# Getting Started.

To configure the environment and test the application, it's necessary to have installed:

- [Docker desktop](https://www.docker.com/products/docker-desktop)
- [Kubernetes lens](https://k8slens.dev)


# Build and Test.
## Docker.
To configure the environment and test the application, it is necessary to follow the following steps.

From the solution folder where the Dockerfile is located, we open a console.
Here, we create the application container image by assigning it a name, in this case **k8dns**:

``docker build -t k8dns .``

We run this image into the Docker container, exposing port 18080 on your host machine (thus to the world) and port 80 on your container:

``docker run -dp 18080:80 k8dns``

Booting the image is not necessary, but we do this to verify that everything runs correctly in our Docker.
So we stopped the service, since we only need the generated image.


## Kubernetes.
Now we have to run the image on our kubernetes. For this we have to execute the following command:

``kubectl run k8dns --image=k8dns --port=80 --hostport=18080 --image-pull-policy=IfNotPresent --labels="app=k8dns"``

    --port = The port that this container exposes
    --hostport = Container port

We expose the service, and we call it **k8dns-service**

``kubectl expose pod k8dns --type=LoadBalancer --name=k8dns-service``


# Contribute

You can try with postman json that is in the project.

Add everything you want but document it please 😄



