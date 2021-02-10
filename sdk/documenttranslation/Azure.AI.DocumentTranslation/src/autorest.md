# Azure.AI.DocumentTranslation

Run `dotnet build /t:GenerateCode` to generate code.

### AutoRest Configuration
> see https://aka.ms/autorest

``` yaml
input-file:
    -  https://github.com/Azure/azure-rest-api-specs/blob/d6e60da2c05d341a2926fa9fec0e566f14e8e9ad/specification/cognitiveservices/data-plane/TranslatorText/preview/v1.0-preview.1/TranslatorBatch.json
```

### Make generated models internal by default

``` yaml
directive:
  from: swagger-document
  where: $.definitions.*
  transform: >
    $["x-accessibility"] = "internal"
```

### Add nullable annotations
This is to guarantee that we don't introduce breaking changes now that we autogerate the code.
``` yaml
directive:
  from: swagger-document
  where: $.definitions.DetectedLanguage
  transform: >
    $.properties.name["x-nullable"] = true;
    $.properties.iso6391Name["x-nullable"] = true;
```

``` yaml
directive:
  from: swagger-document
  where: $.definitions.LanguageInput
  transform: >
    $.properties.id["x-nullable"] = true;
    $.properties.text["x-nullable"] = true;
```

``` yaml
directive:
  from: swagger-document
  where: $.definitions.MultiLanguageInput
  transform: >
    $.properties.id["x-nullable"] = true;
    $.properties.text["x-nullable"] = true;
```
