
## Messenger

The messenger class is a simple implementation of the famous notification pattern where you establish a 
communication framework between objects without them being coupled or without them knowing about each other.
This is being addressed through a subscriber/publisher notifications system in which any object of a certain
class can publish a notification of a certain type and this notification will be forwarded to other subscriber
objects through the messenger which reduces coupling between parts of the software. The messenger maintains the 
mappings between the subscribers and to which notification they are subscribing through a simple dictionary that 
uses another predefined type as a dictionary key MessengerKey (to store information about the subscriber and the 
type of notification) that can be iterated on to notify those subscribers of any notification is being published.  

There is one more information that is maintained by the messenger while establishing the relations between the 
subscribers and the publishers which is the context of the subscription. The context is an optional parameter 
used while subscribing to a type of notification and when publishing a notification and it is used to 
limit the publishing of the notification to the subscribers that has subscribed on passing the same 
context. The context is preferred to be a value type object or an enumeration to be able to be compared
directly.

![messenger]([http://url/to/img.png](https://drive.google.com/file/d/1nSFdpC_EYlRjBj1GziPK0pSevR-xIL6b/view?usp=sharing))
