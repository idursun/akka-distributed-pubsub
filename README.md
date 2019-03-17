# akka-distributed-pubsub
Minimal distributed pub/sub demo with akka

Demonstrates the usage of Akka.Cluster with DistributedPubSub extension.

## Node.Seed
Seed node for the cluster to be formed. It's a well known address where each node connects to. It's served on port 8081.

## Node.ClusterMonitor
A member node which connects to the cluster and prints the current members of the cluster every 10 seconds. Starts with a random port.

## Sidecar
A very basic hacked sidecar class which can be used to hide the details of connecting to a cluster.

## Sidecar.ConsumerDemo
A console application to demonstrate consuming pubsub messages from a topic inside the cluster

## Sidecar.ProducerDemo
A console application to demonstrate publishing messages to a topic inside the cluster.

## Shared
A class library to define the POCOs for shared events.

# Usage
Start the Node.Seed and then the other apps. 
