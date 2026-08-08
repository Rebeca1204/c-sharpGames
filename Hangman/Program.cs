// Versão 1 - console game like hangman with especified words
/*
using System;

string[] hangs = {
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"          ║   " + '\n' +
	@"          ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══", 
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"          ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      |\  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"       \  ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
    @"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",};

string[] deathHangs = {@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"     ███  ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      o>  ║   " + '\n' +
	@"     /|   ║   " + '\n' +
	@"      >\  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"     <o   ║   " + '\n' +
	@"      |\  ║   " + '\n' +
	@"     /<   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"     /|\  ║   " + '\n' +
	@"     / \  ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      o   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      o   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      o   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      |   ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      /   ║   " + '\n' +
	@"      \   ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    |__   ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    \__   ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"   ____   ║   " + '\n' +
	@"    ══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"    __    ║   " + '\n' +
	@"   /══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"    _ '   ║   " + '\n' +
	@"  _/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      _   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      _   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@"      _   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      .   ║   " + '\n' +
	@"          ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      '   ║   " + '\n' +
	@" __/══════╩═══",
	@"      ╔═══╗   " + '\n' +
	@"      |   ║   " + '\n' +
	@"      O   ║   " + '\n' +
	@"          ║   " + '\n' +
	@"          ║   " + '\n' +
	@"      _   ║   " + '\n' +
	@" __/══════╩═══",};

int currentHang = 0;

Random random = new Random();
string[] words = {"WORD", "PHRASE", "FRIEND", "PLACE", "VEHICLE", "EXAMPLE", "ORDENATION"};
string word = words[random.Next(0,words.Length)];
char[] wordChars = word.ToCharArray();

char[] guess = new char[wordChars.Length];
for (int i = 0; i < wordChars.Length; i++){
    guess[i] = '_';
}

do {
    Console.Clear();
    DrawHang();

    Console.WriteLine("\nGuess a letter: ");
    string l = Console.ReadLine().ToUpper();
    while (!Char.IsLetter(l[0])){
        Console.WriteLine("Guess a letter: ");
        l = Console.ReadLine().ToUpper();
    }

    int count = 0;
    for(int i = 0; i < wordChars.Length; i++){
        if (wordChars[i] == l[0]){
            guess[i] = l[0];
        }
        else count++;
    }
    if (count == wordChars.Length) currentHang++;

    if (currentHang ==  6){
        for (int i = 0; i < deathHangs.Length; i++){
            Console.Clear();
            Console.Write(deathHangs[i]);
            Thread.Sleep(500);
        }
        Console.WriteLine("You Lose!");
        Console.WriteLine($"The word was {word}");
        break;
    }

    count = 0;
    for (int i =0; i< guess.Length; i++){
        if (wordChars[i] == guess[i]) count++;
    }
    if (count == wordChars.Length){
        DrawHang();
        Console.WriteLine("You win!");
        break;
    } 

}while(true);

void DrawHang(){
    Console.Write(hangs[currentHang]);
    foreach (char letter in guess){
        Console.Write($"{letter} ");
    }
}
*/
//Versão 2 - 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.CursorVisible  = false;

var banco = new Dictionary<string, List<(string palavra, string dica)>>
{
    ["ANIMAIS"] = new()
    {
        ("CACHORRO",   "Melhor amigo do homem"),
        ("BORBOLETA",  "Inseto com asas coloridas"),
        ("CROCODILO",  "Réptil de rio com dentes afiados"),
        ("ELEFANTE",   "O maior animal terrestre"),
        ("GIRAFA",     "Animal de pescoço longo"),
        ("TARTARUGA",  "Réptil de carapaça dura"),
        ("PAPAGAIO",   "Ave que imita a fala humana"),
        ("PINGUIM",    "Ave que não voa e vive no frio"),
        ("RINOCERONTE","Mamífero com chifre no focinho"),
        ("TUBARAO",    "Peixe predador dos mares"),
    },
    ["PAÍSES"] = new()
    {
        ("BRASIL",     "País do carnaval e do futebol"),
        ("ARGENTINA",  "País do tango e do mate"),
        ("PORTUGAL",   "País de onde veio nossa língua"),
        ("JAPAO",      "Terra do sol nascente"),
        ("AUSTRALIA",  "País-continente no hemisfério sul"),
        ("CANADA",     "Segundo maior país do mundo em área"),
        ("NORUEGA",    "País dos fiordes e da aurora boreal"),
        ("EGITO",      "País das pirâmides e do Nilo"),
        ("MEXICO",     "País do guacamole e das mariachis"),
        ("INDIA",      "País mais populoso do mundo"),
    },
    ["TECNOLOGIA"] = new()
    {
        ("ALGORITMO",  "Conjunto de instruções para resolver um problema"),
        ("COMPILADOR", "Transforma código-fonte em executável"),
        ("INTERFACE",  "Ponto de interação entre sistemas ou usuário"),
        ("RECURSAO",   "Quando uma função chama a si mesma"),
        ("BANCO",      "Sistema que armazena dados organizados"),
        ("VARIAVEL",   "Espaço na memória com um nome e valor"),
        ("PROTOCOLO",  "Conjunto de regras para comunicação em rede"),
        ("SERVIDOR",   "Computador que fornece serviços a outros"),
        ("FRAMEWORK",  "Estrutura de apoio ao desenvolvimento"),
        ("DEPURACAO",  "Processo de encontrar e corrigir erros"),
    },
    ["ESPORTES"] = new()
    {
        ("FUTEBOL",    "Esporte com 11 jogadores e uma bola"),
        ("BASQUETE",   "Esporte com cestas suspensas"),
        ("NATACAO",    "Esporte praticado na água"),
        ("GINASTICA",  "Esporte de equilíbrio e flexibilidade"),
        ("VOLEIBOL",   "Esporte com rede no meio da quadra"),
        ("ATLETISMO",  "Conjunto de provas de pista e campo"),
        ("JUDÔ",       "Arte marcial japonesa"),
        ("CICLISMO",   "Esporte praticado de bicicleta"),
        ("ESGRIMA",    "Esporte de duelo com espadas"),
        ("REMO",       "Esporte praticado em barcos com remos"),
    },
    ["COMIDAS"] = new()
    {
        ("FEIJOADA",   "Prato típico brasileiro com feijão e carne"),
        ("LASANHA",    "Prato italiano de massas empilhadas"),
        ("BRIGADEIRO", "Doce brasileiro de chocolate"),
        ("SUSHI",      "Prato japonês com arroz e peixe cru"),
        ("PAELLA",     "Prato espanhol com arroz e frutos do mar"),
        ("QUIBE",      "Prato de origem árabe com carne moída"),
        ("STROGONOFF", "Prato cremoso de carne com champignon"),
        ("CROISSANT",  "Pão francês em formato de meia-lua"),
        ("COXINHA",    "Salgado brasileiro de frango"),
        ("TAPIOCA",    "Prato típico nordestino de goma de mandioca"),
    },
};

string[] fases = {
    "      ╔═══╗   \n" +
    "      |   ║   \n" +
    "          ║   \n" +
    "          ║   \n" +
    "          ║   \n" +
    "     ███  ║   \n" +
    "    ══════╩═══",

    "      ╔═══╗   \n" +
    "      |   ║   \n" +
    "      O   ║   \n" +
    "          ║   \n" +
    "          ║   \n" +
    "     ███  ║   \n" +
    "    ══════╩═══",

    "      ╔═══╗   \n" +
    "      |   ║   \n" +
    "      O   ║   \n" +
    "      |   ║   \n" +
    "          ║   \n" +
    "     ███  ║   \n" +
    "    ══════╩═══",

    "      ╔═══╗   \n" +
    "      |   ║   \n" +
    "      O   ║   \n" +
    "     /|   ║   \n" +
    "          ║   \n" +
    "     ███  ║   \n" +
    "    ══════╩═══",

    "      ╔═══╗   \n" +
    "      |   ║   \n" +
    "      O   ║   \n" +
    "     /|\\  ║   \n" +
    "          ║   \n" +
    "     ███  ║   \n" +
    "    ══════╩═══",

    "      ╔═══╗   \n" +
    "      |   ║   \n" +
    "      O   ║   \n" +
    "     /|\\  ║   \n" +
    "       \\  ║   \n" +
    "     ███  ║   \n" +
    "    ══════╩═══",

    "      ╔═══╗   \n" +
    "      |   ║   \n" +
    "      O   ║   \n" +
    "     /|\\  ║   \n" +
    "     / \\  ║   \n" +
    "     ███  ║   \n" +
    "    ══════╩═══",
};

string[] animMorte = {
    "      ╔═══╗   \n" + "      |   ║   \n" + "      O   ║   \n" + "     /|\\  ║   \n" + "     / \\  ║   \n" + "     ███  ║   \n" + "    ══════╩═══",
    "      ╔═══╗   \n" + "      |   ║   \n" + "      O   ║   \n" + "     /|\\  ║   \n" + "     / \\  ║   \n" + "          ║   \n" + "    ══════╩═══",
    "      ╔═══╗   \n" + "      |   ║   \n" + "      o>  ║   \n" + "     /|   ║   \n" + "      >\\  ║   \n" + "          ║   \n" + "    ══════╩═══",
    "      ╔═══╗   \n" + "      |   ║   \n" + "      O   ║   \n" + "     /|\\  ║   \n" + "     / \\  ║   \n" + "          ║   \n" + "    ══════╩═══",
    "      ╔═══╗   \n" + "      |   ║   \n" + "     <o   ║   \n" + "      |\\  ║   \n" + "     /<   ║   \n" + "          ║   \n" + "    ══════╩═══",
    "      ╔═══╗   \n" + "      |   ║   \n" + "      O   ║   \n" + "     /|\\  ║   \n" + "     / \\  ║   \n" + "          ║   \n" + "    ══════╩═══",
    "      ╔═══╗   \n" + "      |   ║   \n" + "      o   ║   \n" + "      |   ║   \n" + "      |   ║   \n" + "          ║   \n" + "    ══════╩═══",
    "      ╔═══╗   \n" + "      |   ║   \n" + "      O   ║   \n" + "          ║   \n" + "      /   ║   \n" + "      \\   ║   \n" + "    ══════╩═══",
    "      ╔═══╗   \n" + "      |   ║   \n" + "      O   ║   \n" + "      '   ║   \n" + "          ║   \n" + "    |__   ║   \n" + "    ══════╩═══",
    "      ╔═══╗   \n" + "      |   ║   \n" + "      O   ║   \n" + "      .   ║   \n" + "      '   ║   \n" + "   ____   ║   \n" + "  /══════╩═══",
};

var rng      = new Random();
int  pontos  = 0;
int  recorde = 0;
int  partida = 1;
bool jogar   = true;

TelaTitulo();

while (jogar)
{
    var (dificuldade, maxErros) = EscolherDificuldade();
    var (categoria, palavra, dica) = SortearPalavra();

    bool ganhou = Jogar(palavra, dica, categoria, dificuldade, maxErros);

    if (ganhou)
    {
        int bonus = CalcularBonus(dificuldade, palavra.Length);
        pontos += bonus;
        if (pontos > recorde) recorde = pontos;
        TelaVitoria(palavra, bonus);
    }
    else
    {
        TelaDerrota(palavra);
        pontos = Math.Max(0, pontos - 50);
    }

    partida++;
    jogar = PerguntarJogarNovamente();
}

TelaFinal();

void TelaTitulo()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(@"
   ███████╗ ██████╗ ██████╗  ██████╗ █████╗
   ██╔════╝██╔═══██╗██╔══██╗██╔════╝██╔══██╗
   █████╗  ██║   ██║██████╔╝██║     ███████║
   ██╔══╝  ██║   ██║██╔══██╗██║     ██╔══██║
   ██║     ╚██████╔╝██║  ██║╚██████╗██║  ██║
   ╚═╝      ╚═════╝ ╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.ResetColor();
    Console.WriteLine("Categorias: Animais · Países · Tech · Esportes · Comidas ");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("\nPressione qualquer tecla para começar...");
    Console.ResetColor();
    Console.CursorVisible = false;
    Console.ReadKey(true);
}

(string dif, int maxErros) EscolherDificuldade()
{
    int sel = 0;
    var opcoes = new (string nome, string desc, int erros, ConsoleColor cor)[]
    {
        ("FÁCIL",   "8 tentativas  · dica sempre visível",  8, ConsoleColor.Green),
        ("MÉDIO",   "6 tentativas  · dica sob pedido",       6, ConsoleColor.Yellow),
        ("DIFÍCIL", "4 tentativas  · sem dica",              4, ConsoleColor.Red),
    };

    while (true)
    {
        Console.Clear();
        Titulo("ESCOLHA A DIFICULDADE", ConsoleColor.Cyan);
        Console.WriteLine();

        for (int i = 0; i < opcoes.Length; i++)
        {
            bool ativo = i == sel;
            Console.ForegroundColor = ativo ? ConsoleColor.Black : opcoes[i].cor;
            Console.BackgroundColor = ativo ? opcoes[i].cor : ConsoleColor.Black;
            Console.WriteLine($"  {(ativo ? "▶" : " ")} {opcoes[i].nome,-10} {opcoes[i].desc}");
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nEnter confirmar");
        Console.ResetColor();

        var k = Console.ReadKey(true).Key;
        if (k == ConsoleKey.UpArrow)   sel = (sel - 1 + opcoes.Length) % opcoes.Length;
        if (k == ConsoleKey.DownArrow) sel = (sel + 1) % opcoes.Length;
        if (k == ConsoleKey.Enter) return (opcoes[sel].nome, opcoes[sel].erros);
    }
}

(string cat, string palavra, string dica) SortearPalavra()
{
    var cats = banco.Keys.ToList();
    string cat  = cats[rng.Next(cats.Count)];
    var entry   = banco[cat][rng.Next(banco[cat].Count)];
    string palavra = NormalizarAcentos(entry.palavra).ToUpper();
    return (cat, palavra, entry.dica);
}

bool Jogar(string palavra, string dica, string categoria, string dificuldade, int maxErros)
{
    var letrasCorretas = new HashSet<char>();
    var letrasErradas  = new HashSet<char>();
    bool dicaUsada     = false;
    bool dicaVisivel   = dificuldade == "FÁCIL";

    while (true)
    {
        DesenharTela(palavra, categoria, dificuldade, dica,
                     letrasCorretas, letrasErradas, dicaVisivel, maxErros);

        if (palavra.All(c => c == ' ' || letrasCorretas.Contains(c)))
            return true;

        if (letrasErradas.Count >= maxErros)
        {
            AnimarMorte();
            return false;
        }

        Console.CursorVisible = true;
        Console.SetCursorPosition(2, Console.CursorTop);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("  Digite uma letra");
        if (dificuldade == "MÉDIO" && !dicaUsada)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" (ou '?' para dica)");
        }
        Console.Write(": ");
        Console.ResetColor();

        string entrada = (Console.ReadLine() ?? "").Trim().ToUpper();
        Console.CursorVisible = false;

        if (entrada == "?" && dificuldade == "MÉDIO" && !dicaUsada)
        {
            dicaVisivel = true;
            dicaUsada   = true;
            pontos     = Math.Max(0, pontos - 20); // penalidade de pedir dica
            continue;
        }

        if (entrada.Length == 0 || !char.IsLetter(entrada[0])) continue;

        char letra = NormalizarAcentos(entrada)[0];

        if (letrasCorretas.Contains(letra) || letrasErradas.Contains(letra)) continue;

        if (palavra.Contains(letra))
            letrasCorretas.Add(letra);
        else
            letrasErradas.Add(letra);
    }
}

void DesenharTela(string palavra, string categoria, string dificuldade,
                  string dica, HashSet<char> corretas, HashSet<char> erradas,
                  bool dicaVisivel, int maxErros)
{
    Console.Clear();

    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine($"  Partida #{partida}   ", Console.WindowWidth);
    Console.SetCursorPosition(Console.WindowWidth - 30, 0);
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"Pontos: {pontos,5}  ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write($"Recorde: {recorde,5}");
    Console.ResetColor();
    Console.WriteLine();

    EscreverCor(new string('─', Console.WindowWidth - 1), ConsoleColor.DarkGray);
    Console.WriteLine();

    int erros = erradas.Count;
    int fase  = Math.Min(erros, fases.Length - 1);

    double risco = (double)erros / maxErros;
    ConsoleColor corForca = risco < 0.4 ? ConsoleColor.Green
                          : risco < 0.7 ? ConsoleColor.Yellow
                          : ConsoleColor.Red;

    Console.ForegroundColor = corForca;
    Console.WriteLine(fases[fase]);
    Console.ResetColor();

    Console.Write("  Categoria: ");
    EscreverCor($"[{categoria}]", ConsoleColor.Cyan);
    Console.Write("   Dificuldade: ");
    EscreverCor($"[{dificuldade}]", dificuldade == "FÁCIL" ? ConsoleColor.Green
                                  : dificuldade == "MÉDIO"  ? ConsoleColor.Yellow
                                  : ConsoleColor.Red);
    Console.WriteLine();

    if (dicaVisivel)
    {
        Console.Write("  Dica: ");
        EscreverCor(dica, ConsoleColor.DarkYellow);
        Console.WriteLine();
    }
    else if (dificuldade == "MÉDIO")
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  Dica: ? (digite '?' para revelar -20pts)");
        Console.ResetColor();
    }
    Console.WriteLine();

    Console.Write("  ");
    foreach (char c in palavra)
    {
        if (c == ' ')
        {
            Console.Write("  ");
        }
        else if (corretas.Contains(c))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{c} ");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("_ ");
            Console.ResetColor();
        }
    }
    Console.WriteLine();
    Console.WriteLine();

    Console.Write("  Tentadas: ");
    foreach (char c in erradas.OrderBy(x => x))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{c} ");
    }
    foreach (char c in corretas.OrderBy(x => x))
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{c} ");
    }
    Console.ResetColor();
    Console.WriteLine();

    Console.WriteLine();
    Console.Write("  Vidas: ");
    for (int i = 0; i < maxErros; i++)
    {
        if (i < maxErros - erros)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("♥ ");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("♡ ");
        }
    }
    Console.ResetColor();
    Console.WriteLine($"  ({maxErros - erros} restante{(maxErros - erros == 1 ? "" : "s")})");
    Console.WriteLine();
}

void AnimarMorte()
{
    foreach (var frame in animMorte)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(frame);
        Console.ResetColor();
        Thread.Sleep(350);
    }
}

void TelaVitoria(string palavra, int bonus)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n  A palavra era: [{palavra}]");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"  +{bonus} pontos de bônus!");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"  Total: {pontos} pontos  |  Recorde: {recorde}");
    Console.ResetColor();
    Thread.Sleep(500);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("\n  Pressione qualquer tecla...");
    Console.ResetColor();
    Console.ReadKey(true);
}

void TelaDerrota(string palavra)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Red;
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n  A palavra era: [{palavra}]");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"  -50 pontos de penalidade");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"  Total: {pontos} pontos  |  Recorde: {recorde}");
    Console.ResetColor();
    Thread.Sleep(500);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("\n  Pressione qualquer tecla...");
    Console.ResetColor();
    Console.ReadKey(true);
}

bool PerguntarJogarNovamente()
{
    Console.Clear();
    Titulo("JOGAR NOVAMENTE?", ConsoleColor.Cyan);
    Console.WriteLine();

    int sel = 0;
    while (true)
    {
        Console.SetCursorPosition(0, 4);
        for (int i = 0; i < 2; i++)
        {
            bool ativo = i == sel;
            string label = i == 0 ? "Sim, jogar mais!" : "Não, encerrar";
            Console.ForegroundColor = ativo ? ConsoleColor.Black : ConsoleColor.White;
            Console.BackgroundColor = ativo ? ConsoleColor.Cyan : ConsoleColor.Black;
            Console.WriteLine($"  {(ativo ? "▶" : " ")} {label}          ");
            Console.ResetColor();
        }
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n  ↑↓ navegar   Enter confirmar");
        Console.ResetColor();

        var k = Console.ReadKey(true).Key;
        if (k == ConsoleKey.UpArrow)   sel = (sel - 1 + 2) % 2;
        if (k == ConsoleKey.DownArrow) sel = (sel + 1) % 2;
        if (k == ConsoleKey.Enter)     return sel == 0;
    }
}

void TelaFinal()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"\n  Partidas jogadas : {partida - 1}");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"  Pontuação final  : {pontos}");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"  Recorde          : {recorde}");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("\n  Até a próxima!\n");
    Console.ResetColor();
}
int CalcularBonus(string dificuldade, int tamanho) =>
    dificuldade switch
    {
        "FÁCIL"   => 50  + tamanho * 5,
        "MÉDIO"   => 100 + tamanho * 10,
        "DIFÍCIL" => 200 + tamanho * 20,
        _         => 50
    };

void Titulo(string texto, ConsoleColor cor)
{
    string linha = new string('═', texto.Length + 4);
    Console.ForegroundColor = cor;
    Console.WriteLine($"  ╔{linha}╗");
    Console.WriteLine($"  ║  {texto}  ║");
    Console.WriteLine($"  ╚{linha}╝");
    Console.ResetColor();
}

void EscreverCor(string texto, ConsoleColor cor)
{
    Console.ForegroundColor = cor;
    Console.Write(texto);
    Console.ResetColor();
}

string NormalizarAcentos(string s)
{
    var sb = new System.Text.StringBuilder();
    foreach (char c in s.Normalize(System.Text.NormalizationForm.FormD))
    {
        var cat = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
        if (cat != System.Globalization.UnicodeCategory.NonSpacingMark)
            sb.Append(c);
    }
    return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
}