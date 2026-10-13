using System;
using System.Collections.Generic;

namespace ProjetoBiblioteca
{
    public class Emprestimo
    {
        private DateTime dtEmprestimo;
        private DateTime dtDevolucao;

        public DateTime DtEmprestimo { get => dtEmprestimo; set => dtEmprestimo = value; }
        public DateTime DtDevolucao { get => dtDevolucao; set => dtDevolucao = value; }
    }

    public class Exemplar
    {
        private int tombo;
        private List<Emprestimo> emprestimos;

        public int Tombo { get => tombo; set => tombo = value; }
        public List<Emprestimo> Emprestimos => emprestimos;

        public Exemplar(int tombo)
        {
            this.tombo = tombo;
            this.emprestimos = new List<Emprestimo>();
        }

        public bool Disponivel()
        {
            if (emprestimos.Count == 0) return true;
            foreach (var e in emprestimos)
            {
                if (e.DtDevolucao == DateTime.MinValue || e.DtDevolucao > DateTime.Now)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Emprestar()
        {
            if (Disponivel())
            {
                emprestimos.Add(new Emprestimo { DtEmprestimo = DateTime.Now, DtDevolucao = DateTime.MinValue });
                return true;
            }
            return false;
        }

        public bool Devolver()
        {
            foreach (var e in emprestimos)
            {
                if (e.DtDevolucao == DateTime.MinValue || e.DtDevolucao > DateTime.Now)
                {
                    e.DtDevolucao = DateTime.Now;
                    return true;
                }
            }
            return false;
        }

        public int QtdeEmprestimos()
        {
            return emprestimos.Count;
        }
    }

    public class Livro
    {
        private int isbn;
        private string titulo;
        private string autor;
        private string editora;
        private List<Exemplar> exemplares;

        public int Isbn { get => isbn; set => isbn = value; }
        public string Titulo { get => titulo; set => titulo = value; }
        public string Autor { get => autor; set => autor = value; }
        public string Editora { get => editora; set => editora = value; }
        public List<Exemplar> Exemplares => exemplares;

        public Livro(int isbn, string titulo, string autor, string editora)
        {
            this.isbn = isbn;
            this.titulo = titulo;
            this.autor = autor;
            this.editora = editora;
            this.exemplares = new List<Exemplar>();
        }

        public void AdicionarExemplar(Exemplar exemplar)
        {
            exemplares.Add(exemplar);
        }

        public int QtdeExemplares()
        {
            return exemplares.Count;
        }

        public int QtdeDisponiveis()
        {
            int count = 0;
            foreach (var ex in exemplares)
            {
                if (ex.Disponivel()) count++;
            }
            return count;
        }

        public int QtdeEmprestimos()
        {
            int count = 0;
            foreach (var ex in exemplares)
            {
                count += ex.QtdeEmprestimos();
            }
            return count;
        }

        public double PercDisponibilidade()
        {
            if (QtdeExemplares() == 0) return 0;
            return ((double)QtdeDisponiveis() / QtdeExemplares()) * 100;
        }
    }

    public class Livros
    {
        private List<Livro> acervo = new List<Livro>();

        public List<Livro> Acervo => acervo;

        public void Adicionar(Livro livro)
        {
            acervo.Add(livro);
        }

        public Livro Pesquisar(Livro livro)
        {
            foreach (var l in acervo)
            {
                if (l.Isbn == livro.Isbn || l.Titulo.Equals(livro.Titulo, StringComparison.OrdinalIgnoreCase))
                {
                    return l;
                }
            }
            return null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Livros biblioteca = new Livros();
            int opcao;

            do
            {
                Console.WriteLine("\n=== OPÇÕES DO SELETOR - BIBLIOTECA ===");
                Console.WriteLine("0. Sair");
                Console.WriteLine("1. Adicionar livro");
                Console.WriteLine("2. Pesquisar livro (sintético)");
                Console.WriteLine("3. Pesquisar livro (analítico)");
                Console.WriteLine("4. Adicionar exemplar");
                Console.WriteLine("5. Registrar empréstimo");
                Console.WriteLine("6. Registrar devolução");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida!");
                    continue;
                }

                switch (opcao)
                {
                    case 0:
                        Console.WriteLine("A sair...");
                        break;

                    case 1:
                        Console.Write("ISBN: ");
                        int isbn = int.Parse(Console.ReadLine());
                        Console.Write("Título: ");
                        string titulo = Console.ReadLine();
                        Console.Write("Autor: ");
                        string autor = Console.ReadLine();
                        Console.Write("Editora: ");
                        string editora = Console.ReadLine();

                        Livro l = new Livro(isbn, titulo, autor, editora);
                        biblioteca.Adicionar(l);
                        Console.WriteLine("Livro adicionado com sucesso!");
                        break;

                    case 2:
                        Console.Write("ISBN ou Título do livro: ");
                        string buscaSintetico = Console.ReadLine();
                        int.TryParse(buscaSintetico, out int isbnBusca);
                        Livro livroSintetico = biblioteca.Pesquisar(new Livro(isbnBusca, buscaSintetico, "", ""));

                        if (livroSintetico != null)
                        {
                            Console.WriteLine($"\n--- DADOS SINTÉTICOS ---");
                            Console.WriteLine($"Título: {livroSintetico.Titulo} | Autor: {livroSintetico.Autor} | Editora: {livroSintetico.Editora}");
                            Console.WriteLine($"Total de Exemplares: {livroSintetico.QtdeExemplares()}");
                            Console.WriteLine($"Exemplares Disponíveis: {livroSintetico.QtdeDisponiveis()}");
                            Console.WriteLine($"Total de Empréstimos: {livroSintetico.QtdeEmprestimos()}");
                            Console.WriteLine($"Percentual de Disponibilidade: {livroSintetico.PercDisponibilidade():F2}%");
                        }
                        else
                        {
                            Console.WriteLine("Livro não encontrado.");
                        }
                        break;

                    case 3:
                        Console.Write("ISBN ou Título do livro: ");
                        string buscaAnalitico = Console.ReadLine();
                        int.TryParse(buscaAnalitico, out int isbnBuscaAna);
                        Livro livroAnalitico = biblioteca.Pesquisar(new Livro(isbnBuscaAna, buscaAnalitico, "", ""));

                        if (livroAnalitico != null)
                        {
                            Console.WriteLine($"\n--- DADOS ANALÍTICOS ---");
                            Console.WriteLine($"Título: {livroAnalitico.Titulo} | Autor: {livroAnalitico.Autor} | Editora: {livroAnalitico.Editora}");
                            Console.WriteLine($"Total: {livroAnalitico.QtdeExemplares()} | Disponíveis: {livroAnalitico.QtdeDisponiveis()} | % Disp: {livroAnalitico.PercDisponibilidade():F2}%");
                            Console.WriteLine("Exemplares:");
                            foreach (var ex in livroAnalitico.Exemplares)
                            {
                                Console.WriteLine($" - Tombo: {ex.Tombo} | Disponível: {ex.Disponivel()} | Empréstimos: {ex.QtdeEmprestimos()}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Livro não encontrado.");
                        }
                        break;

                    case 4:
                        Console.Write("ISBN do livro para adicionar exemplar: ");
                        int isbnEx = int.Parse(Console.ReadLine());
                        Livro livroEx = biblioteca.Pesquisar(new Livro(isbnEx, "", "", ""));

                        if (livroEx != null)
                        {
                            Console.Write("Número do tombo do exemplar: ");
                            int tombo = int.Parse(Console.ReadLine());
                            Exemplar ex = new Exemplar(tombo);
                            livroEx.AdicionarExemplar(ex);
                            Console.WriteLine("Exemplar adicionado com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("Livro não encontrado.");
                        }
                        break;

                    case 5:
                        Console.Write("ISBN do livro: ");
                        int isbnEmp = int.Parse(Console.ReadLine());
                        Livro livroEmp = biblioteca.Pesquisar(new Livro(isbnEmp, "", "", ""));

                        if (livroEmp != null)
                        {
                            bool emprestado = false;
                            foreach (var ex in livroEmp.Exemplares)
                            {
                                if (ex.Emprestar())
                                {
                                    Console.WriteLine($"Empréstimo efetuado com sucesso no tombo: {ex.Tombo}");
                                    emprestado = true;
                                    break;
                                }
                            }
                            if (!emprestado)
                            {
                                Console.WriteLine("Não há exemplares disponíveis para empréstimo.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Livro não encontrado.");
                        }
                        break;

                    case 6:
                        Console.Write("ISBN do livro: ");
                        int isbnDev = int.Parse(Console.ReadLine());
                        Livro livroDev = biblioteca.Pesquisar(new Livro(isbnDev, "", "", ""));

                        if (livroDev != null)
                        {
                            Console.Write("Número do tombo do exemplar para devolução: ");
                            int tomboDev = int.Parse(Console.ReadLine());
                            Exemplar exDev = null;
                            foreach (var ex in livroDev.Exemplares)
                            {
                                if (ex.Tombo == tomboDev)
                                {
                                    exDev = ex;
                                    break;
                                }
                            }

                            if (exDev != null)
                            {
                                if (exDev.Devolver())
                                {
                                    Console.WriteLine("Devolução registada com sucesso!");
                                }
                                else
                                {
                                    Console.WriteLine("Este exemplar não se encontra emprestado.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Exemplar não encontrado.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Livro não encontrado.");
                        }
                        break;

                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

            } while (opcao != 0);
        }
    }
}
