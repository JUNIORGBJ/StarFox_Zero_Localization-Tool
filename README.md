<h1 align="center"><figure>
  <img src="Fox.PNG">
</figure></h1>

<p align="center">
  Editor WinForms para arquivos <code>.mcd</code> de <strong>Star Fox Zero</strong>, com foco em localização, revisão de textos, remapeamento de charset e suporte a pacotes <code>.dat</code>.
</p>

<p align="center">
  <img alt="Platform" src="https://img.shields.io/badge/Platform-Windows-0078D4?style=for-the-badge">
  <img alt=".NET" src="https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge">
  <img alt="UI" src="https://img.shields.io/badge/UI-WinForms-0C7CD5?style=for-the-badge">
  <img alt="Visual Studio" src="https://img.shields.io/badge/Visual%20Studio-2022-5C2D91?style=for-the-badge">
</p>

## Visão Geral

O **StarFox Zero Localization Tool** foi criado para facilitar a edição dos arquivos de texto do jogo com uma interface nativa em **C# + WinForms**.

Além da edição de strings, o projeto também oferece ferramentas para:

- validar cobertura de charset;
- importar e exportar textos via CSV;
- remapear caracteres e flags de idioma;
- criar e ajustar novos glyphs;
- exportar e reimportar texturas associadas ao atlas de fonte;
- extrair e reempacotar arquivos `.dat`, `.dtt`, `.eff` e `.evn`.

## Destaques

- Interface desktop nativa, sem dependências web
- Compatível com o Designer do Visual Studio 2022
- Fluxo direto para abrir, editar, validar e salvar arquivos `.mcd`
- Round-trip de tradução por CSV
- Preview visual do atlas e dos glyphs
- Ferramenta integrada para reempacotamento de arquivos de jogo

## Recursos

### Editor de MCD

- abertura e salvamento de arquivos `.mcd`;
- fechamento do arquivo atual com reset completo do estado da UI;
- navegação por eventos e strings em árvore;
- edição direta do texto selecionado;
- busca textual com navegação entre resultados;
- substituição individual e em lote.

### Localização e charset

- validação de caracteres ausentes no charset;
- exportação das strings para CSV;
- importação de traduções a partir de CSV;
- remapeamento de caractere-fonte para caractere-destino;
- atualização de flags de idioma do caractere;
- criação de novos caracteres com seleção visual da área do glyph;
- ajuste fino de largura, altura e posição da seleção.

### Texturas e preview

- preview do atlas de textura vinculado ao glyph;
- visualização ampliada da região selecionada;
- exportação da textura para DDS;
- importação de DDS preservando o fluxo atual da ferramenta.

### Arquivos DAT

A ferramenta auxiliar de arquivos DAT permite:

- extrair arquivos `.dat`, `.dtt`, `.eff` e `.evn`;
- reempacotar respeitando o layout original.

## Stack

- **C#**
- **.NET 9.0**
- **Windows Forms**
- **Visual Studio 2022**

## Como Compilar

### Visual Studio 2022

1. Abra `StarFoxZeroLocalizationTool.sln`
2. Selecione a configuração desejada
3. Compile normalmente pelo Visual Studio

## Fluxo de Uso

1. Abra um arquivo `.mcd`
2. Navegue pelas entradas na árvore lateral
3. Edite os textos desejados
4. Valide o charset para localizar caracteres faltantes
5. Exporte ou importe CSV quando necessário
6. Ajuste glyphs e texturas do atlas, se o arquivo exigir
7. Salve o novo `.mcd`
8. Se estiver trabalhando com pacotes do jogo, use a ferramenta de `.dat` para extrair ou reempacotar

## Diferenciais do Projeto

- feito para uso prático em modding e localização;
- interface orientada a produtividade, sem depender de scripts para operações comuns;
- preserva um fluxo confortável para edição visual de fontes;
- base preparada para evolução contínua em WinForms.

## Licença

Este projeto está licenciado sob a **Licença MIT**.

Você é livre para usar, copiar, modificar, mesclar, publicar, distribuir, sublicenciar e vender cópias do software, desde que o aviso de copyright e o texto da licença sejam incluídos em todas as cópias ou partes substanciais do software.

Consulte o arquivo [`LICENSE`](LICENSE) para obter o texto completo da licença.

Copyright (c) 2026 Junior GBJ
