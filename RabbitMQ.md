# RabbitMQ:

### Summary:

```
RabbitMQ allows services to publish and consume messages of each other.
When a service publishes a message, the subscriber/consumer service can get the message when it's back online.

Acknowledgment:
Consumers can acknowledge the receipt and processing of a message. Once a message is acknowledged, RabbitMQ can remove it from the queue. If a consumer fails to process a message, it can be requeued for delivery to another consumer.
```

- <b>RabbitMQ has 4 several methods. Every method has:</b><br>
  a publisher (e.g.: PlatformService)<br>
  a consumer (e.g.: CommandService)<br>
  a MessageBus or Broker (Exchange, Queue)<br><br>

---

### RabbitMQ methods:

#### 1. RabbitMQ Direct Exchange

- Deliver Messages to queue based on routing key<br>
- Ideal for "direct" or unicast messaging<br>

#### 2. RabbitMQ Fanout Exchange

- Deliver Messages to all queues that are bound to the exchange<br>
- It ignores the routing key<br>
- Ideal for broadcast messages<br>
- The publisher doesn't care who is listening. It just throws messages<br>

#### 3. RabbitMQ Topic Exchange

- Routes messages to 1 or more queues based on the routing key (and patterns)<br>
- Used for Multicast messaging<br>
- Implements various Publisher / Subscriber Patterns<br>

---

### RabbitMQ Web API setup (.NET7)

How RabbitMQ is architected ?<br>
RabbitMQ is a multi-layer architecture. (Connection setup, channel setup, exchange setup).<br>
Check <b>'MessageBusClient'</b> class.

### After RabbitMQ Web API setup (.NET7)

Now it's time to use it from within the controller so that when I create a platform, it will send a synchronous message to the platform service, and drop a message to the MessageBus (to get towards asynchronous messaging between the two services).<br>
