# C# Console Games

Coleção de **10 jogos clássicos** implementados do zero em C#, rodando direto no terminal.

![C#](https://img.shields.io/badge/language-C%23-239120?logo=csharp)
![.NET](https://img.shields.io/badge/.NET-6%2B-512BD4?logo=dotnet)
![Platform](https://img.shields.io/badge/platform-console-lightgrey)
![Games](https://img.shields.io/badge/jogos-10-blue)
![Status](https://img.shields.io/badge/Status-Active-brightgreen?style=for-the-badge)

---

## Jogos disponíveis

| # | Jogo | Versão atual | Destaques da versão mais recente |
|---|------|:---:|---|
| 1 | [Battleship](#-battleship) | v2 | Grade 10×10, 5 navios nomeados, IA Hunt-Target |
| 2 | [DiceGame](#-dicegame) | v2 | Saldo, apostas, item "Sorte Grande" |
| 3 | [FlapBird](#-flapbird) | v2 | Física, obstáculos móveis, loja de skins, moedas |
| 4 | [GuessANumber](#-guessanumber) | v2 | Cronômetro, dicas inteligentes, ranking em arquivo |
| 5 | [Hangman](#-hangman) | v2 | Categorias, dificuldade, pontuação, PT-BR |
| 6 | [Pacman](#-pacman) | v2 | Rastro corrigido, fuga real dos fantasmas |
| 7 | [Quiz](#-quiz) | v2 | 4 modos, streak multiplier, "Meu Material" via .txt |
| 8 | [RockPaperScissors](#️-rockpaperscissors) | v1 | Placar de rodadas |
| 9 | [Simon](#-simon) | v1 | Painel ASCII colorido, sequência crescente |
| 10 | [TicTacToe](#-tictactoe) | v3 | Minimax com poda α-β, 3 dificuldades |

---
 
## Como executar
 
**Pré-requisito:** [.NET 6 SDK](https://dotnet.microsoft.com/download) ou superior.
 
```bash
# Clone o repositório
git clone https://github.com/Rebeca1204/c-sharpGames.git
cd c-sharpGames
 
# Entre na pasta do jogo desejado e execute
cd Hangman
dotnet run
```
 
---

---
 
## Detalhes
 
### Battleship
 
Batalha naval contra o computador em uma grade 10×10.
 
**Evolução:**
- **v1** — Grade 5×5, navios em posições numéricas, IA aleatória
- **v2** — Grade 10×10, 5 navios com nomes e tamanhos distintos (Porta-aviões ao Patrulha), posicionamento com orientação H/V, IA com modo Hunt-Target que afunila o ataque após o primeiro acerto
**Como jogar:** posicione seus navios escolhendo linha (A–J) e coluna (1–10) com orientação H ou V. Na fase de combate, informe coordenadas para atirar. Afunde toda a frota inimiga primeiro.
 
**Conceitos:** matrizes 2D, OOP, IA Hunt-Target, gerenciamento de estado
 
---
 
### DiceGame
 
**Evolução:**
- **v1** — Jogador e computador rolam dados em 10 rodadas; maior soma vence a rodada
- **v2** — Simulador de cassino: saldo inicial de $100, apostas livres por rodada, escolha de 1–3 dados, item "Sorte Grande" (+3 ao total, uso único)
**Como jogar:** defina quantos dados usar, aposte seu saldo a cada rodada e tente lucrar. O item "Sorte Grande" pode virar uma rodada difícil.
 
**Conceitos:** `Random`, loops, validação de input, gerenciamento de recursos
 
---
 
### FlapBird
 
Clone do Flappy Bird rodando no terminal.
 
**Como jogar:** pressione `Espaço` ou `↑` para bater as asas e evitar os canos. Colete moedas `$` que aparecem entre os obstáculos. Desbloqueie skins na loja com as moedas acumuladas.
 
**Destaques:**
- Física com gravidade e trail de partículas
- Canos que se movem verticalmente após 10 pontos
- Velocidade crescente a cada 2 pontos
- 5 skins desbloqueáveis com preços em moedas
- High score e moedas salvos em arquivo entre sessões
**Conceitos:** animação em console, física simples, `Thread`, persistência em arquivo, loja com itens
 
---
 
### GuessANumber
 
**Evolução:**
- **v1** — Número secreto com dicas "maior/menor" e pontuação decrescente
- **v2** — Dicas de temperatura (Queimando → Muito frio), modo Contra o Tempo (45s), dicas especiais de emergência (paridade, dezena, soma de dígitos), ranking persistente em arquivo
**Como jogar:** escolha a dificuldade (Fácil/Médio/Difícil/Contra o Tempo) e tente adivinhar o número. Erros consomem tentativas; acertos rápidos dão bônus de tempo.
 
**Conceitos:** `Stopwatch`, `Thread` para timer paralelo, ranking em arquivo, dicas contextuais
 
---
 
### Hangman
 
**Evolução:**
- **v1** — Palavras fixas em inglês, 6 erros máximos, animação de morte ASCII
- **v2** — 50 palavras em 5 categorias (Animais, Países, Tecnologia, Esportes, Comidas), 3 dificuldades com menus navegáveis por seta, dica opcional com custo de pontos, letras coloridas por acerto/erro, forca que muda de cor conforme o risco, normalização de acentos
**Como jogar:** use `↑↓` para navegar nos menus e `Enter` para confirmar. Digite uma letra por vez. No modo Médio, `?` revela a dica por -20 pontos.
 
**Conceitos:** `Dictionary`, `HashSet`, normalização Unicode, menus interativos com teclas
 
---
 
### Pacman
 
Pac-Man no terminal com labirinto gerado proceduralmente.
 
**Evolução:**
- **v1** — Coleta de pontos, fantasmas, power-up — mas com rastro visual acumulando na tela
- **v2** — Correção do rastro (posições anteriores apagadas a cada tick, comida restaurada ao apagar célula ocupada por fantasma), fuga dos fantasmas com direção real (canto oposto ao jogador), wrap-around corrigido nas bordas
**Como jogar:** `←↑↓→` para mover. Colete todos os pontos sem ser pego. `★` ativa poder e permite eliminar fantasmas por 200pts.
 
**Conceitos:** matrizes 2D, renderização seletiva no console, `SetCursorPosition`, algoritmo de fuga
 
---
 
### Quiz
 
**Evolução:**
- **v1** — Pergunta o código ASCII de letras aleatórias, encerra no primeiro erro
- **v2** — 4 modos: ASCII completo (letras + especiais + conceitos), C#/.NET (20 perguntas técnicas), Misto, e **"Meu Material"** (carrega qualquer arquivo `.txt` local); cronômetro de 15s por questão, sistema de streak com multiplicador (2× e 3×), ranking persistente, análise de dificuldade do material carregado
**Como jogar:** escolha o modo, responda com `A/B/C/D` (múltipla escolha) ou digitando (discursiva). Respostas rápidas dão bônus de tempo.
 
**Modo "Meu Material":** crie um arquivo `.txt` na pasta do jogo seguindo o formato:
```
# comentário
P: Sua pergunta aqui
R: Resposta correta
O: Resposta correta|Opção 2|Opção 3|Opção 4
D: Explicação opcional
```
 
**Conceitos:** OOP com herança (`Question`, `MC`, `Free`), Minimax para CPU, `Thread` para timer, persistência em arquivo, parser de formato customizado
 
---
 
### RockPaperScissors
 
Pedra, papel e tesoura contra o computador com placar acumulado por rodadas.
 
**Como jogar:** escolha `1` (Pedra), `2` (Papel) ou `3` (Tesoura). O placar é mantido durante a sessão.
 
**Conceitos:** `enum`, `Random`, lógica de vitória
 
---
 
### Simon
 
Clone do Simon Says com painel de botões desenhado em ASCII e cores distintas.
 
**Como jogar:** observe a sequência de cores exibida e repita-a com `↑` `→` `↓` `←`. A cada rodada a sequência cresce. Um erro encerra a partida.
 
**Conceitos:** `List<int>` para sequência dinâmica, `ConsoleColor`, `Thread.Sleep` para animação
 
---
 
### TicTacToe
 
**Evolução:**
- **v1** — IA totalmente aleatória, input direto
- **v2** — IA bloqueia linha iminente do humano e tenta vencer, sorteio de quem começa, placar acumulado
- **v3** — **Minimax com poda α-β** (IA imbatível no modo Difícil), 3 dificuldades, modo 2 jogadores com nomes, destaque visual da linha vencedora, estatísticas por sessão
**Como jogar:** escolha o modo (vs CPU ou 2 jogadores) e a dificuldade. Digite 1–9 para marcar a posição no tabuleiro. No modo Difícil, o melhor resultado possível contra a IA é empate.
 
**Conceitos:** Minimax com poda α-β, OOP com polimorfismo (`Jogador`, `JogadorHumano`, `JogadorComputador`), `Dictionary` para estatísticas
 
---

## Estrutura do repositório

```
c-sharpGames/
├── Battleship/
│   └── Program.cs
├── DiceGame/
│   └── Program.cs
├── FlapBird/
│   └── Program.cs
├── GuessANumber/
│   └── Program.cs
├── Hangman/
│   └── Program.cs
├── Pacman/
│   └── Program.cs
├── Quiz/
│   └── Program.cs
├── RockPaperScissors/
│   └── Program.cs
├── Simon/
│   └── Program.cs
└── TicTacToe/
    └── Program.cs
```
