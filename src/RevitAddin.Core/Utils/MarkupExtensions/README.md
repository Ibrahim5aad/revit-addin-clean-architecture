
## MethodBindingExtension, EventArgsExtension, EventSenderExtension

The spirit behind those markup extensions is to be able to reach a higher degree of separation and modularity between the View and the ViewModel. Before addressing the idea behind those markup extensions, let us clarify a little bit how the XAML parser work. When you set an attribute value in a XAML file, the initial type of that value is a pure string text. Even other primitive types such as Int, [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double) are initially parsed as strings and passed to a XAML processor. In order to resolve this string text value, the processor needs to know about the type of the property that is being set; for primitive types a direct conversion is being applied by the XAML parser and if it is an enumeration type (i.e. HorizontalAlignment and VerticalAlignment properties) the string is used to check for an enumeration member with the same name. 

The complication comes in when the property value type is not a primitive type or an enumeration, then the type in question must be able to resolve the XAML string text value and provide an instance of itself. This -most of the time- happens behind the scenes with a **TypeConverter** class at use; it is a helper class that the used for providing values/instances of other types in XAML code. 

An example of this scenario happening behind the scenes is when you use a linear gradient brush, with a start point and an end point that defines the gradient slope:

**<LinearGradientBrush StartPoint="0,0" EndPoint="1,1"/>**


StartPoint and EndPoint properties are of type **Point** that is defined in **System.Windows** namespace, but here in XAML what is being assigned to the properties is merely strings. Those strings are getting resolved behind the scenes with the help of **PointConverter** class that is defined in the same namespace. Without the existence of type converters you will have to write something that is more verbose like this:

**<LinearGradientBrush>**

`  `**<LinearGradientBrush.StartPoint>**

`    `**<Point X="0" Y="0"/>**

`  `**</LinearGradientBrush.StartPoint>**

`  `**<LinearGradientBrush.EndPoint>**

`    `**<Point X="1" Y="1"/>**

`  `**</LinearGradientBrush.EndPoint>**

**</LinearGradientBrush>**




Markup extensions are in a general sense exist to tackle the same problem. But the difference between type converters and markup extensions can be clarified by this quote from Microsoft Docs: 

“However, there are scenarios where different behavior is required. For example, a XAML processor can be instructed that a value of an attribute should not result in a new object in the object graph. Instead, the attribute should result in an object graph that makes a reference to an already constructed object in another part of the graph, or a static object. Another scenario is that a XAML processor can be instructed to use a syntax that provides non-default arguments to the constructor of an object. These are the types of scenarios where a markup extension can provide the solution.”


**MarkupExtension** is a class that can be implemented to provide values for properties in XAML. There are a lot of markup extensions that is shipped with the WPF framework and anyone used them at some stage, the most famous on is **Binding**. Binding provides a data bound value for a property, using the data context that applies to the parent object at run time.

So, getting back to the problem we have here; how can you bind a method that is in the view model (if you are applying MVVM) to an event. With the previous introduction we saw how markup extensions can be of a great help for providing values of types other than primitives in XAML. The answer to this problem or how can you convert the method to an event handler that can be subscribed to the event at runtime, is by using an implementation of a markup extensions to get you the job done. Markup extensions derived classes must implement the 


**ProvideValue** method that takes as an argument an object of type **IServiceProvider** which will be provided by the binding engine, this object is used to provide services to the markup extension class. With the help of this service provide we can retrieve a WPF service that is retrieved by the type **IProvideValueTarget** that can be used to retrieve the target object of the binding which at this case any framework element that uses the markup extensions which after this we can access its DataContext property to access methods in the view model.

The method binding extension can be used like this: 

**xmlns:markup**="clr-namespace:MarkupExtensions"

<**ComboBox** 

` `**Template**="{DynamicResource ComboBoxControlTemplate}"

` `**ItemContainerStyle**="{DynamicResource ItemStyle}" 

` `**SelectionChanged**="{markup:MethodBinding SomethingChanged,  
`                   `{markup:EventArgs}}" **/ComboBox>**


As you can see the first parameter to the method binding extension is the method name string, and the second one is another markup extension that captures the event args. You can pass any arguments you like. You can also specify the binding target as the first argument and the markup extension will always check if the first argument passed is a string (will be used as the method name) or another object that will be used as a binding target instead of the resolved data context.


One issue that will occur with this implementation is that when trying to find the method in question in the resolved target object data context, reflection is being used to look for method that matches the name and the signature provided in XAML and if the DLL is obfuscated afterwards what obfuscation does to the method names is that it changes the class members names to a randomly generated names, and this will break the above-mentioned implementation that will not find a method with the name provided in XAML. Another class comes in which soles this issue; the **BindableMethodAttribute.** This is an attribute that takes a string as parameter and will be used to decorate the methods that is eligible for binding through the MethodBinding extension in XAML. With this attribute we the previous implementation is modified and instead of looking for a method with a specific name, the markup extension will look for a method that is decorated with this attribute. The string passed to the attribute will have to match the name passed to the method extensions in XAML.

`    `[**BindableMethod**("SomethingChanged")]

`    `public void SomethingChanged()

`    `{
`      `///Do something
`    `}

![](Aspose.Words.09c701a4-8860-4a85-a7d5-270e86a02371.001.png)

