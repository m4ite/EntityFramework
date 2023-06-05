using System;
using System.Linq;
using System.Threading.Tasks;
using EntityTest.Model;


TestentityContext context = new TestentityContext();

await createUser("Don");
await createUser("Marcão");
await createUser("Queila Lima");
await createUser("Pamella");

await createProduct("Bico", "O máximo do avanço tecnológico em mecânica.");
await createProduct("Pudim", "O máximo do avanço tecnológico em gastronomia.");

var users =
    from user in context.Usuarios
    where user.Nome == "Queila Lima"
    select user;

var queila = users.FirstOrDefault();


// -------------------
var produtos =
    from product in context.Produtos
    where product.Nome == "Pudim"
    select product;

var pudim = produtos.FirstOrDefault();

// OU

//var pudim = context.Produtos
    //.FirstOrDefault(x => x.Nome == "Pudim");

// -------------------

await addOfertum(pudim, queila, 10);

printData();

async Task addOfertum(Produto produto, Usuario usuario, decimal value)
{
    Ofertum oferta = new Ofertum();

    oferta.Usuario = usuario.Id;
    oferta.Produto = produto.Id;
    oferta.Preco = value;
    context.Oferta.Add(oferta);

    await context.SaveChangesAsync();
}

async Task createProduct(string nome, string desc)
{
    Produto produto = new Produto();

    produto.Nome = nome;
    produto.Descricao = desc;
    context.Produtos.Add(produto);

    await context.SaveChangesAsync();
}

void printData()
{
    var query =
        from user in context.Usuarios
        join ofer in context.Oferta
        on user.Id equals ofer.Usuario
        select new
        {
            nome = user.Nome,
            produto = ofer.Produto,
            valor = ofer.Preco
        } into x // into -> coloca o objeto anonimo na variavel x 
        join prod in context.Produtos
        on x.produto equals prod.Id
        select new
        {
            nome = x.nome,
            produto = prod.Nome,
            valor = x.valor
        };

    foreach (var y in query)
        Console.WriteLine($"{y.nome} está vendendo um {y.produto} por {y.valor} R$.");
}

async Task createUser(string nome)
{
    Usuario usuario = new Usuario();

    usuario.Nome = nome;
    usuario.DataNascimento = DateTime.Now;
    usuario.Foto = null;
    context.Usuarios.Add(usuario);

    await context.SaveChangesAsync();
}