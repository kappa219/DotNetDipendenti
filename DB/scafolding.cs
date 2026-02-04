// lo scafolding è  operazione inversa alla migrazione
// con lo scafolding si genera il codice C# a partire da un database esistente
// comando da terminale per generare lo scafolding
// dotnet ef dbcontext scaffold "server=localhost;port=3306;database=corso_csharp;user=root;password=;
//" Pomelo.EntityFrameworkCore.MySql -o DB --context ApplicationDbContext --context-dir Data -f
// -o specifica la cartella di output
// --context specifica il nome del contesto da generare
// --context-dir specifica la cartella dove mettere il contesto
// -f forza la sovrascrittura dei file esistenti




// le librerie entity framework core per mysql
// Pomelo.EntityFrameworkCore.MySql

// MySql.EntityFrameworkCore

// Microsoft.EntityFrameworkCore.Tools

//*------------------------------------------------------------------------------

/* 
dotnet ef dbcontext scaffold "Server=localhost;Database=nomedb;User=root;Password=password;
" Pomelo.EntityFrameworkCore.MySql -o Models






                                                    */