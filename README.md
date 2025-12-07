# 📖 Resource Manager 📖

This Manager was originally created to manage languages in my ***WPF*** applications. <br>
It also works on other kind of resources tho. 

## 📚 Lets go through a **step** by **step** tutorial on how to setup and use this library. 

### 📋 Setup
Open the project terminal and type in ***dotnet add package Cacx.LanguageManager --version 1.0.0***

### 💻 Usage
*Optional:* <br>

Register the LocalizationService via *DI* <br>
*You can also implement your own LocalizationService and register it via the Interface*
```cs
public App()
{
    AppHost = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
    {
      _ = services.AddSingleton<ILocalizationService, LocalizationService>();
    }
}
```
### Step 1

#### ➡️ Get the *ILocalizationService* implementation
**There a two ways to achieve this.** <br>

Either get it from your *DI* Container
```cs
public ExampleCtor(ILocalizationService service)
{
  
}
```

or instanciate an *ILocalizationService* one time... 

```cs
//Call this at the start of your application
public void Method()
{
  _ = new LocalizationService("some random path...") //You dont have to use the instance instantly you can disregrad it for now
}
```

and use this property afterwards to get it...
```cs
public void SomeOtherMethod()
{
  ILocalizationService service = LocalizationProvider.Service;
}
```

### Step 2

#### 🌐 Set the Language
**This will be CultureInfo.CurrentUICulture by default**

```cs
public void Example()
{
  CultureInfo language = new CultureInfo("en-US");
  service.SetLanguage(language);
}
```

### Step 3

#### 🔁Update/Set the Context

**Imagine you have this folder structure (which is recommended)**

```cs
///   MyApp(Project)
///    ->Resources(Folder)
///        ->Login(Folder)
///            Login.resx
///            Login.de-DE.resx
///        ->CreateAccount(Folder)
///            CreateAccount.resx
///            CreateAccount.de-DE.resx

➡️ Imagine you wanna access the Login resource
public void Example()
{
  string basePath = "MyApp.Resources.Login.Login"; //"{ProjectName}.{Folder}.{Folder}.{Filename}"
  service.UpdateContext(basePath);
}
```

**You have to do this context switch to access the resources of another file!**
