# Karma

Simple binary serialization library made for personal projects. No advantages over other libs. Powered by NIH-syndrome.

## Usage

### Serialization
```
new KarmaSerializer(stream).Write(obj)
```

### Deserialization
```
TOut obj = new KarmaDeserializer(stream).Read<TOut>()
```

## Supported Types

* Int16
* Int32
* Int64
* UInt16
* UInt32
* UInt64
* Double
* Single
* Byte
* Sigend Byte
* String (UTF8 encoded only)
* GUID
* List
* Dictionary
* Array
* Class
