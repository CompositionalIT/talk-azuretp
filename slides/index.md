- title : Compositional IT talk
- description : Template for CIT talk
- author : CIT Person
- theme : night
- transition : default

***

# Taming Types in the Cloud

***

## About me

---

## I'm Isaac Abraham!

![](images/isaac.jpg)

---

Other metadata...

* .NET dev / contractor / consultant since .NET 1.0
* Using F# for ~5 years
* Microsoft MVP (F# -> .NET -> Visual Studio -> ?)
* Based in Fulda, Germany (and occasionally London, UK)

---

## I also make infamous PRs...

![](images/nugate.png)

---

## What about you?

***

![](images/fsharp256.png)

---

* General purpose programming language
* Functional-*first*
* Powerful type system
* Awesome data manipulation capabilities
* Leads to the "pit of success"

---

![](images/POS-1.png)

---

![](images/POS-2.png)

---

| C# / VB .NET | F#
|-:|:-
| Mutable by default | Immutable by default
| Side-effects and statements | Expressions
| Classes | Functions as values
| Inheritance | Composition
| State | Pure functions
| Polymorphism | Algebraic Data Types

---

## F# Primer in < 5 minutes

---

### Values

```fsharp
// bind 5 to x
let x:int = 5

// type inference
let inferredX = 5

// functions are just values, don't need a class
let helloWorld(name) = sprintf "Hello, %s" name 

// type inference again
let text = helloWorld "isaac"
```

---

### Types

```fsharp
open System

// Tuples are first class citizens in F#
let person = Tuple.Create("Isaac", 36)
let personShortHand = "isaac", 36 // string * int
let name, age = personShortHand // decompose the tuple 

// Declaring a record
type Person = { Name : string; Age : int }

// Create an instance
let me = { Name = "Isaac"; Age = 36 }
printfn "%s is %d years old" me.Name me.Age
```
---

### More Types

```fsharp
type Direction = North | South | East | West
type Weather =
    | Cold of temperatureC:float
    | Sunny
    | Wet
    | Windy of Direction * windspeed:float

// Create a weather value
let weather = Windy(North, 10.2)

let (|Low|Medium|High|) speed =
    if speed > 10. then High elif speed > 5. then Medium else Low

match weather with
| Cold temp when temp < 2.0 -> "Really cold!"
| Cold _ | Wet -> "Miserable weather!"
| Sunny -> "Nice weather"
| Windy (North, High) -> "High speed northernly wind!"
| Windy (South, _) -> "Blowing southwards"
| Windy _ -> "It's windy!"
```

---

### Asynchronous support

```fsharp
open System
open System.Net

let webPageSize = async {
    use wc = new WebClient()
    let! result = wc.AsyncDownloadString(Uri "http://www.bbc.co.uk")
    return result.Length }
```

---

### ACHTUNG!

![alt](images/horror.jpg)

---

### Whitespace sensitive 

```fsharp
open System
    
let prettyPrintTime() =
    let time = DateTime.UtcNow
    printfn "It is now %d:%d" time.Hour time.Minute
```
---

### Equals is comparison!

```fsharp
let x = 5
x = 10 // false, COMPARISON!!!
```
---

### Immutable by default

```fsharp
let a = 10
//a <- 20 // not allowed

let mutable y = 10 // need an extra keyword!
y <- 20 // ok 
```

---

### REPL

* Read, Evaluate, Print Loop
* No console applications needed
* Scripts
* Explore domain quickly
* Converts quickly to full-blown assemblies

---

### Also....

* Type Inference *everywhere*
* Expressions *everywhere*
* **Type Providers**

***

![](images/azure.png)

---

* Lower costs
* Reduce cap ex
* "Scale fast, fail fast" etc. 
* Reduce time to market
* Enable distributed applications

---

* Deploying
* Replication
* Load balancing
* Logging
* Scale up
* Scale out
* Auth
* Resiliency

---

## Azure and F#?

![alt](images/suspicious.png)

---

![](images/friends.jpg)

---

<a href="https://docs.microsoft.com/en-us/dotnet/fsharp/using-fsharp-on-azure/">
    <img src="images/azure-docs-1.png" style="width: 700px;"/>
</a>

---

## F# runs on .NET!

---

| Cloud Applications | F# |
|---:|:---|
Stateless | Immutable, Expressions
Data-centric | Type Providers, Pattern Matching
Fault tolerant | Powerful type system
Asynchronous | async { }
Distributed | cloud { }

***

## Data in Azure

---

## Azure Storage

![](images/azure-storage.jpg)

---

### SQL Azure

![](images/azure-sql.png)

---

### And others...

* Cosmos DB
* Redis Cache
* Data Lake
* etc. etc.

---

### Comparing SQL, Tables and Blobs

| | SQL Azure | Tables | Blobs |
|---:|:---:|:---:|:---:|
**Type System** | Strong | Weak | Weaker
**Query Language** | Smart | Dumb | Dumber  
**Cost** | High | Low | Low
**Scalability** | Medium (*) | Medium/High | Medium/High
**Performance** | Scalable | Fixed (*) | Fixed (*)

---

### Working with SQL Azure

* *Table*-level schema
* Relatively rich type system

Customer Id | Name | Order Count | Balance
| --- | --- | --- | ---|
| `GUID` | `string 50 null` | `int` | `decimal` |
| 2542685a-... | Joe Bloggs | 23 | 126.23
| bcf678fb-... | Sally Smith | 12 | 59.10
| ad081c1b-... | ***{null}*** | 17 | 89.23

---

### Working with Azure Tables

* *Row*-level schema
* Type checking
* No max length etc.
* Implicit nullability "support"


    { CustomerId = "2542685a"; Name = "Joe"; OrderCount = 23 }
    { CustomerId = 123; Name = "Sally"; OrderCount = 12; Balance = 59.10 }
    { CustomerId = 123; OrderCount = 12; Balance = 59.10; City = "New York" }

---

Customer Id | Name | Order Count | Balance | **City**
| --- | --- | --- | ---| --- 
| 2542685a-... | Joe Bloggs | 23 | ***{N/A}*** | ***{N/A}***
| **123** | Sally Smith | 12 | 59.10 | ***{N/A}***
| ad081c1b-... | ***{N/A}*** | 17 | 89.23 | **New York** 

---

## Blob Type System

* *No* schema
* No notion of rows / columns
* Data stored as raw documents *e.g.*


        { "customerId" : "2542685a-"
          "name" : "Joe Bloggs"
          "orderCount" : 23 }

---

### Compute on Azure data services

| | SQL | Tables | Blobs
---:|:---:|:---:|:---:| 
**Projection** | Yes | Yes | No
**Filters** | Yes | *Limited* | No
**Joins** | Yes | *No* | No
**Relationships** | Yes | *No* | No
**Indexes** | Yes | *Limited* | Limited

***

### DEVELOPERS!

![](images/developers.jpg)

---

### Problems with Azure Storage SDK

* Not geared up for exploration
* Impedence mismatch of type system and services
* Does not lead to pit of success

---

### Typical tasks:

* What's in my storage account?
* What does this file look like inside?
* What's the schema of this table?
* What does the data in my table look like?
* What's currently on the queue?

### Question:
### How can we use F# to improve this?

---

![](images/bored.jpg)

---

# DEMOS!!

***

### Applications of Storage Type Provider

* Application code for Tables
* Exploration of data
    * Log tables
    * Metrics
    * Exploring "heads" of blobs
    * Exploring unseen tables
* Working with "well-know" blob schemas
* Can always "fall back" to standard Azure SDK

***

## Storage Accounts cost money!

* **Every** query costs
    * **Every** time you dot into something, it costs!
* Use the Storage Emulator
* Use dummy data locally to "script" the schema you want to work with
* Repoint to live storage account at runtime

***

## Developing Type Providers

* It's a pain
* It's a REAL pain
* Debugging is next to impossible
* Running 2 IDEs side-by-side
* Slow to develop
* Dependencies are difficult to work with

***

## Build and CI

![](images/storage-tp-build.png)

***

## Building and Testing

* Tests = compiles :)
    * Need to have the emulator running locally!
* Suite of integration tests on CI
* Appveyor for build
    * Appveyor support Azure Storage Emulator :)

***


## Founder of Compositional IT

<img src="images/CIT-Horizontal.png" style="width: 700px;"/>

***

## Thank you!

<a href="https://compositional-it.com">
    <img src="images/CIT-Circle.png" style="width: 200px;"/>
</a>

https://compositional-it.com

[@isaac_abraham](http://twitter.com/isaac_abraham)

[isaac@compositional-it.com](mailto:isaac@compositional-it.com)
