# Messaging

Synchronous & Asynchronous Messaging

### Synchronous Messaging

- Request / Response Cycle
- Requester will "wait" for response
- Externally facing services usually synchronous (e.g. http requests)
- Services usually need to "know" about each other
- We are using 2 forms: Http, gRPC (Google Remote Procedure Call)

### Asynchronous Messaging

- No Request / Response Cycle
- Requestor does not wait
- Event model, e.g. publish - subscribe
- Typically used between services
- Event bus is often used (we'll be using RabbitMQ)
- Services don't need to know about each other, just the bus
- Introduces its own range of complexities, not a magic bullet
