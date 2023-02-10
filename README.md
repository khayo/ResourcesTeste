# ResourcesTeste

# Motivação
Criei este documento para registrar algo não encontrei um bom material compilado para ler. Então reuni todas as informações que encontrei e organizei de forma mais lógica, para facilitar a vida de quem precisar utilizar este recurso.

# Aplicação 
- Web APIs dotnet 6 e 7 (foco do material)
- Web Applications dotnet 6 e 7

# Incluindo middleware de localização
Primeiro é necessário configurar a classe Program com as informações de Localização
**Importante**: a posição da inclusão dos parâmetros é relevante
 ![AddLocalization](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/f3ed5899c8534c8395ad9642db7ea61d.png)
```cs
builder.Services.AddLocalization();
``` 

![SupportedCultures](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/65042f97cb954f2689639435cb6fc741.png)

```cs
var supportedCultures = new[] { "en", "pt-BR", "pt-PT", "es", "fr", "ru" };

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
```
## Detalhamento
**var supportedCultures = new[] { "en", "pt-BR", "pt-PT", "es", "fr", "ru" };** -  A lista de idiomas que estarão disponíveis. A lista de language tags está disponivel [aqui](https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c)

**var localizationOptions = new RequestLocalizationOptions()** - Objeto sendo instânciado

**.SetDefaultCulture("en")** -  É possivel passar o parametro de duas formas, ou como uma string, ou passando o array criado anteriormente  indicando o índice da linguagem que será o padrão, por exemplo **supportedCultures\[0]**. Este será o idioma padrão da API quando não for passado nenhum idioma

**.AddSupportedCultures(supportedCultures)** - indica para qual idioma o sistema deve alterar a apresentação de formatação de datas, números, moedas, etc.

**.AddSupportedUICultures(supportedCultures);** - Indica quais os idiomas vão ter suporte dentro da API para tradução de termos.


# Arquivo de resources
Com a API configurada devemos criar os arquivos de resources, existem 3 abordagens
## Nomeando por Path
Um arquivo relacionado a cada Controller ou View criado na mesma estrutura de pastas. Ex: Controllers/Calculos/Soma.en.resx

| ![NomePorPath](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/775167131a3743d7b8ff2ec017d2fc1a.png) |
| --- |
| *Arquivos de recursos sendo criados com a mesma estrutura de pasta da Controller e da View* |

## Resource por "dot file"
- Um arquivo para cada Controller todos na mesma pasta, nomeados com a estrutura de pastas da controler separada por pontos. Ex: Controllers.Calculos.Soma.en.resx

| ![NomePorDotFile](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/c6a112305f0b42b48bf0c829b1a6deef.png) |
| --- |
| *Arquivo de recursos indicando o caminho no nome do arquivo, separando nome das pastas por pontos* |

## Um único arquivo para todas as Controllers
Criar uma pasta ou Projeto onde a classe de recursos e os recursos serão armazenados.

| ![EstruturaDasPastas](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/f6a141cc81074aa0a285c7dfd84fbfac.png) |
| --- |
| *Pasta onde a classe de recursos doi criada* |

| ![ClasseDummy](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/016107bb15e34cc7ae98dc5e702f68cd.png) |
| --- | 
| *Classe dummy de recursos, vazia para agregar os arquivos de recursos* |


| ![CriandoResource](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/0f2982da27b3474e9cabc81c68ad0f65.png) |
| --- |
| *Nessa pasta onde foi criado a classe Resource, cria-se um arquivo de recursos* |

| ![NomeandoResource](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/2d159b67d8654641b7ec14bee12a8efa.png) |
|---|
| *Adicionando a sigla do idioma ao nome do resource, ele automaticamente deve se aninhar a classe Resource.* |

|![RecursosCriados](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/baccddf1f5354c678ec7a1f8d55fd614.png)|
| --- |
|*Arquivos aninhados*|

# Dentro do Resource
| ![DentroDoResource](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/51ffa95b31b548bb95e91958798702a3.png)|
| --- |
| O arquivo de recursos tem 3 campos, Name (chave), Value (valor), Comment (comentário) |
# Aplicando Localization
**Obs**: Vou focar na explicação do uso da terceira forma de uso de recursos, um arquivo único para todas as controllers.

Dentro da controller é necessário injetar uma Interface de **IStringLocalizer** do tipo da classe criada anteriormente, no caso deste exemplo, Resource e inicializar o objeto no construtor.
![InjetandoIStringLocalizer](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/2c7c2d0bc7764114bc21034e9583c592.png)
```cs
	private readonly IStringLocalizer<Resource> _localizer;

	public ResoucesTesteController(IStringLocalizer<Resource> localizer)
	{
		_localizer = localizer;
	}
```

Para utilizar o localizer, basta chamar a instância injetada, passando a chave do resource
![UsandoLocalizerChave](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/4ef818e64d9b4e1c9c6ee17ecea10b9e.png)
``` csharp
	[HttpGet("ResourceCadeira")]
	public string ResourceCadeira()
	{
		return _localizer["stringCadeira"];
	}
```

Também é possivel recuperar a lista de strings existencer no resource:
![BuscandoListaDeResources](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/a4bfc3df7c8f46ca9b30951d87ec6a15.png)
```cs
	[HttpGet("ResourceStringList")]
	public List<string> ResourceStringList()
	{
		return _localizer.GetAllStrings().ToList().Select(s => s.Value).ToList();
	}
```

Ou ainda uma lista de objetos do tipo **LocalizedString**
![buscandoObjetos](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/e3ca9a9a06bb48839265358779816d07.png)
```cs
	[HttpGet("ResourceList")]
	public List<LocalizedString> ResourceList()
	{
		return _localizer.GetAllStrings().ToList();
	}
```

# Como utilizar
Ao fazer a requisição na API, adicione a querystring **?culture=fr** no final do link para o endpoint utilizar o idioma indicado. Alguns exemplos de uso:

| ![usandoFr](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/849cef5ec3e54371bf55f87df6caab63.png) |
| --- |
| Utilizando o idioma Francês |

| ![usandoRu](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/7baa4d90c53641f2bacd6e67e96bf528.png) |
| --- |
| Utilizando o idioma Russo |

| ![usandoPtBr](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/9897b7d7e2f4457a8bef04be3c4167c8.png) |
| --- |
| Utilizando o idioma pt-BR |

| ![default](https://dicasdopinguim.com.br/wp-content/uploads/2023/02/5e208e84dd85460bbff4c5d576650f2e.png) |
| --- |
| Utilizando o idioma padrão do sistema |

# Conclusão
Meu caso de uso é tradução de mensagens de erros enviadas pela API, facilita bastante o dia a dia, pois preciso me preocupar apenas com as mensagens no meu idioma e criar chaves para estas mensagens.

# Docker
Para rodar na máquina, provavelmente será necessário instalar o docker. Se você não usa docker, recomendo fortemente aprender nem que seja o básico, foi um divisor de águas na minha vida profissional.
