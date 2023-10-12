# RabbitMQ:

- <b>RabbitMQ has 4 several methods. Every method has:</b><br><br>
  a publisher (e.g.: PlatformService)<br>
  a consumer (e.g.: CommandService)<br>
  a Message Broker (Exchange, Queue)<br><br>

---

- <b>RabbitMQ has several methods:</b><br>

### 1. RabbitMQ Direct Exchange

- Deliver Messages to queue based on routing key<br>
- Ideal for "direct" or unicast messaging

---

### 2. RabbitMQ Fanout Exchange

- Deliver Messages to all queues that are bound to the exchange<br>
- It ignores the routing key<br>
- Ideal for broadcast messages<br>
- The publisher doesn't care who is listening. It just throws messages<br>

---

### 3. RabbitMQ Topic Exchange

- Routes messages to 1 or more queues based on the routing key (and patterns)<br>
- Used for Multicast messaging<br>
- Implements various Publisher / Subscriber Patterns<br>

---

## RabbitMQ Web API setup (.NET7)

How RabbitMQ is architected:<br>
RabbitMQ is a multi-layer architecture. (Connection setup, channel setup, exchange setup). Check <b>'MessageBusClient'</b> class.
