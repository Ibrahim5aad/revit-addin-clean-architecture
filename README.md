# Revit Add-in: Clean Architecture
A .NET sample showing a simple layered clean architecture for Revit add-ins development; applying MVVM, IoC  and other relevant design patterns and SOLID principles.


## What's in this sample?

### MethodBindingExtension, EventArgsExtension, EventSenderExtension: 
Method Extensions for method XAML binding and capturing of the event sender and the event args in XAML.

### Messenger:
Publisher/Subscriber communication pattern object.

### IoC Container: 
Inversion of Control container that is also known as DI Container 
is a framework for implementing automatic dependency injection.

### ViewModelBase: 
INotifyPropertyChanged and INotifyDataErrorInfo Implementation.

### RelayCommand: 
ICommand basic and generic implementation. 
