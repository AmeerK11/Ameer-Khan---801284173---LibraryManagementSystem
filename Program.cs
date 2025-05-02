using System;
using System.Collections.Generic;

class LibraryManagementSystem
{
    static List<Book> books = new List<Book>();
    static List<User> users = new List<User>();
    static Dictionary<string, (string, DateTime)> borrowedBooks = new Dictionary<string, (string, DateTime)>();
    static double finePerDay = 1.0;

    static void Main()
    {
        Console.WriteLine("\n=============================");
        Console.WriteLine("    Library Management System    ");
        Console.WriteLine("=============================");
        InitializeSampleData();
        ShowMainMenu();
    }

    static void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine("\n-----------------------------");
            Console.WriteLine(" Main Menu ");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("1. Manage Books");
            Console.WriteLine("2. Manage Users");
            Console.WriteLine("3. Borrow/Return Books");
            Console.WriteLine("4. Manage Borrowed Books");
            Console.WriteLine("5. Search Books");
            Console.WriteLine("6. Exit");
            Console.Write("Select an option: ");
            
            switch (Console.ReadLine())
            {
                case "1": ManageBooks(); break;
                case "2": ManageUsers(); break;
                case "3": BorrowReturnBooks(); break;
                case "4": ManageBorrowedBooks(); break;
                case "5": SearchBooks(); break;
                case "6": Console.WriteLine("Exiting... Goodbye!"); return;
                default: Console.WriteLine("Invalid option. Try again."); break;
            }
        }
    }
    static void SearchBooks()
    {
        Console.WriteLine("Search by: 1. Title  2. Author  3. Genre");
        Console.Write("Enter choice: ");
        string choice = Console.ReadLine();
        Console.Write("Enter search term: ");
        string term = Console.ReadLine().ToLower();

        var results = books.FindAll(b =>
            (choice == "1" && b.Title.ToLower().Contains(term)) ||
            (choice == "2" && b.Author.ToLower().Contains(term)) ||
            (choice == "3" && b.Genre.ToLower().Contains(term))
        );

        if (results.Count > 0)
        {
            Console.WriteLine("\nSearch Results:");
            results.ForEach(Console.WriteLine);
        }
        else
        {
            Console.WriteLine("No matching books found.");
        }
    }    

    static void ManageBooks()
    {
        Console.WriteLine("\n--- Book Management ---");
        Console.WriteLine("1. Add Book\n2. List Books\n3. Remove Book\n4. Back");
        Console.Write("Select an option: ");
        
        switch (Console.ReadLine())
        {
            case "1": AddBook(); break;
            case "2": ListBooks(); break;
            case "3": RemoveBook(); break;
            case "4": return;
            default: Console.WriteLine("Invalid option."); break;
        }
    }

    static void AddBook()
    {
        Console.Write("Enter book title: ");
        string title = Console.ReadLine();
        Console.Write("Enter author: ");
        string author = Console.ReadLine();
        Console.Write("Enter genre: ");
        string genre = Console.ReadLine();
        books.Add(new Book(title, author, genre));
        Console.WriteLine("Book added successfully!");
    }

    static void ListBooks()
    {
        Console.WriteLine("\n--- Book Inventory ---");
        if (books.Count == 0)
        {
            Console.WriteLine("No books available.");
        }
        else
        {
            foreach (var book in books)
                Console.WriteLine(book);
        }
    }

    static void RemoveBook()
    {
        Console.Write("Enter book title to remove: ");
        string title = Console.ReadLine();
        books.RemoveAll(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine("Book removed if it existed.");
    }



    static void ManageUsers()
    {
        Console.WriteLine("\nUser Management");
        Console.WriteLine("1. Register User");
        Console.WriteLine("2. List Users");
        Console.Write("Select an option: ");
        
        switch (Console.ReadLine())
        {
            case "1": RegisterUser(); break;
            case "2": ListUsers(); break;
            default: Console.WriteLine("Invalid option."); break;
        }
    }

    static void RegisterUser()
    {
        Console.Write("Enter user name: ");
        string name = Console.ReadLine();
        Console.Write("Assign role (Admin/Member): ");
        string role = Console.ReadLine();
        users.Add(new User(name, role));
        Console.WriteLine("User registered successfully!");
    }

    static void ListUsers()
    {
        Console.WriteLine("\nRegistered Users:");
        foreach (var user in users)
            Console.WriteLine(user);
    }

    static void BorrowReturnBooks()
    {
        Console.WriteLine("\n--- Borrow/Return Books ---");
        Console.WriteLine("1. Borrow Book\n2. Return Book\n3. Back");
        Console.Write("Select an option: ");
        
        switch (Console.ReadLine())
        {
            case "1": BorrowBook(); break;
            case "2": ReturnBook(); break;
            case "3": return;
            default: Console.WriteLine("Invalid option."); break;
        }
    }

    static void BorrowBook()
    {
        Console.Write("Enter user name: ");
        string userName = Console.ReadLine();
        Console.Write("Enter book title: ");
        string bookTitle = Console.ReadLine();
        
        if (books.Exists(b => b.Title.Equals(bookTitle, StringComparison.OrdinalIgnoreCase)) && !borrowedBooks.ContainsKey(bookTitle))
        {
            borrowedBooks[bookTitle] = (userName, DateTime.Now);
            Console.WriteLine("Book borrowed successfully!");
        }
        else
        {
            Console.WriteLine("Book is not available or already borrowed.");
        }
    }

    static void ReturnBook()
    {
        Console.Write("Enter book title to return: ");
        string bookTitle = Console.ReadLine();
        
        if (borrowedBooks.ContainsKey(bookTitle))
        {
            var (user, borrowDate) = borrowedBooks[bookTitle];
            borrowedBooks.Remove(bookTitle);
            
            int daysBorrowed = (DateTime.Now - borrowDate).Days;
            double fine = daysBorrowed > 14 ? (daysBorrowed - 14) * finePerDay : 0;
            
            Console.WriteLine("Book returned successfully!");
            if (fine > 0)
            {
                Console.WriteLine($"Overdue by {daysBorrowed - 14} days. Fine: ${fine:F2}");
            }
        }
        else
        {
            Console.WriteLine("Book was not borrowed.");
        }
    }

    static void ManageBorrowedBooks()
    {
        Console.WriteLine("\n--- Borrowed Books ---");
        if (borrowedBooks.Count == 0)
        {
            Console.WriteLine("No books are currently borrowed.");
            return;
        }

        foreach (var entry in borrowedBooks)
        {
            var (user, borrowDate) = entry.Value;
            int daysBorrowed = (DateTime.Now - borrowDate).Days;
            double fine = daysBorrowed > 14 ? (daysBorrowed - 14) * finePerDay : 0;
            
            Console.WriteLine($"{entry.Key} is borrowed by {user} since {borrowDate.ToShortDateString()} - Fine: ${fine:F2}");
        }
    }    
    static void InitializeSampleData()
    {
        books.Add(new Book("The Great Gatsby", "F. Scott Fitzgerald", "Fiction"));
        books.Add(new Book("To Kill a Mockingbird", "Harper Lee", "Fiction"));
        books.Add(new Book("1984", "George Orwell", "Dystopian"));
        books.Add(new Book("Pride and Prejudice", "Jane Austen", "Romance"));
        books.Add(new Book("The Catcher in the Rye", "J.D. Salinger", "Fiction"));
        books.Add(new Book("Animal Farm", "George Orwell", "Political Satire"));
        books.Add(new Book("The Hobbit", "J.R.R. Tolkien", "Fantasy"));
        books.Add(new Book("Brave New World", "Aldous Huxley", "Dystopian"));
        books.Add(new Book("The Lord of the Rings", "J.R.R. Tolkien", "Fantasy"));
        books.Add(new Book("The Da Vinci Code", "Dan Brown", "Mystery"));
        books.Add(new Book("The Alchemist", "Paulo Coelho", "Fantasy"));
        books.Add(new Book("The Little Prince", "Antoine de Saint-Exupéry", "Children's Literature")); 
        books.Add(new Book("The Chronicles of Narnia", "C.S. Lewis", "Fantasy"));
        books.Add(new Book("The Kite Runner", "Khaled Hosseini", "Historical Fiction"));


        users.Add(new User("Alice", "Member"));
        users.Add(new User("Ameer", "Admin"));
        users.Add(new User("Bob", "Member"));
        users.Add(new User("Charlie", "Admin"));
    }
}

class Book
{
    public string Title { get; }
    public string Author { get; }
    public string Genre { get; }

    public Book(string title, string author, string genre)
    {
        Title = title;
        Author = author;
        Genre = genre;
    }

    public override string ToString() => $"{Title} by {Author} - Genre: {Genre}";
}

class User
{
    public string Name { get; }

    public User(string name, string role)
    {
        Name = name;
    }

    public override string ToString() => Name;
}
