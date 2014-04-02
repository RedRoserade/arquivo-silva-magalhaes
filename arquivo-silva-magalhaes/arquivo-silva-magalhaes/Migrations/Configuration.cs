namespace ArquivoSilvaMagalhaes.Migrations
{
    using ArquivoSilvaMagalhaes.Models.ArchiveModels;
    using ArquivoSilvaMagalhaes.Models.SiteModels;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ArquivoSilvaMagalhaes.Models.ArchiveDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ArquivoSilvaMagalhaes.Models.ArchiveDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            SeedAuthors(context);
            SeedCollections(context);

            SeedCollaborators(context);
            
        }




        protected void SeedAuthors(ArquivoSilvaMagalhaes.Models.ArchiveDataContext db)
        {
            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "Ant�nio", LastName = "da Silva Magalh�es", BirthDate = new DateTime(1834, 6, 19), DeathDate = new DateTime(1897, 3, 3) },
                new Author { Id = 2, FirstName = "Ant�nio", LastName = "Carrapato", BirthDate = new DateTime(1966, 1, 1), DeathDate = DateTime.Now },
                new Author { Id = 3, FirstName = "Lu�s Filipe", LastName = "de Aboim Pereira", BirthDate = new DateTime(1800, 1, 1), DeathDate = DateTime.Now }
            };

            var authorTexts = new List<AuthorText>
            {
                new AuthorText { LanguageCode = "pt", Nationality = "pt", Author = authors[0], Biography = "Biografia pt.", Curriculum = "Curriculum pt." }
            };

            authors.ForEach(author => db.AuthorSet.AddOrUpdate(a => new { a.LastName, a.FirstName }, author));
            authorTexts.ForEach(text => db.AuthorTextSet.AddOrUpdate(text));

            db.SaveChanges();


        }

        protected void SeedDocuments(ArquivoSilvaMagalhaes.Models.ArchiveDataContext db)
        {
            // TODO
        }

        void SeedCollections(ArquivoSilvaMagalhaes.Models.ArchiveDataContext db)
        {
            db.CollectionSet.AddOrUpdate(
                c => c.OrganizationCode,
                new Collection
                {
                    Id = 1,
                    Authors = new List<Author> { db.AuthorSet.Find(1) },
                    OrganizationCode = "PT/AFSM/COLA",
                    HasAttachments = false,
                    IsVisible = true,
                    Notes = "Notas da cole��o A.",
                    CatalogCode = "COLA",
                    Type = CollectionType.Collection,
                    ProductionDate = DateTime.Now
                },
                new Collection
                {
                    Id = 2,
                    Authors = new List<Author> { db.AuthorSet.Find(2) },
                    OrganizationCode = "PT/AFSM/COLB",
                    HasAttachments = true,
                    IsVisible = true,
                    Notes = "Notas da cole��o B.",
                    CatalogCode = "COLB",
                    Type = CollectionType.Collection,
                    ProductionDate = DateTime.Now
                },
                new Collection
                {
                    Id = 3,
                    Authors = new List<Author> { db.AuthorSet.Find(1) },
                    OrganizationCode = "PT/AFSM/FUNA",
                    HasAttachments = false,
                    IsVisible = true,
                    Notes = "Notas do fundo A.",
                    CatalogCode = "FUNA",
                    Type = CollectionType.Fond,
                    ProductionDate = DateTime.Now
                },
                new Collection
                {
                    Id = 4,
                    Authors = new List<Author> { db.AuthorSet.Find(2) },
                    OrganizationCode = "PT/AFSM/FUNB",
                    HasAttachments = false,
                    IsVisible = true,
                    Notes = "Notas do fundo B.",
                    CatalogCode = "FUNB",
                    Type = CollectionType.Fond,
                    ProductionDate = DateTime.Now
                }
            );

            db.CollectionTextSet.AddOrUpdate(
                new CollectionText
                {
                    Collection = db.CollectionSet.Find(1),
                    LanguageCode = "pt",
                    Title = "Cole��o A",
                    Description = "Descri��o da Cole��o A.",
                    Provenience = "Proveni�ncia da Cole��o A.",
                    AdministrativeAndBiographicStory = "Hist�ria Administrativa e Biogr�fica da Cole��o A.",
                    Dimension = "Dimens�o da Cole��o A.",
                    FieldAndContents = "�mbito e conte�do da Cole��o A.",
                    CopyrightInfo = "Condi��es de Reprodu��o da Cole��o A."
                },
                new CollectionText
                {
                    Collection = db.CollectionSet.Find(2),
                    LanguageCode = "pt",
                    Title = "Cole��o B",
                    Description = "Descri��o da Cole��o B.",
                    Provenience = "Proveni�ncia da Cole��o B.",
                    AdministrativeAndBiographicStory = "Hist�ria Administrativa e Biogr�fica da Cole��o B.",
                    Dimension = "Dimens�o da Cole��o B.",
                    FieldAndContents = "�mbito e conte�do da Cole��o B.",
                    CopyrightInfo = "Condi��es de Reprodu��o da Cole��o B."
                },
                new CollectionText
                {
                    Collection = db.CollectionSet.Find(3),
                    LanguageCode = "pt",
                    Title = "Fundo A",
                    Description = "Descri��o do Fundo A.",
                    Provenience = "Proveni�ncia do Fundo A.",
                    AdministrativeAndBiographicStory = "Hist�ria Administrativa e Biogr�fica do Fundo A.",
                    Dimension = "Dimens�o do Fundo A.",
                    FieldAndContents = "�mbito e conte�do do Fundo A.",
                    CopyrightInfo = "Condi��es de Reprodu��o do Fundo A."
                },
                new CollectionText
                {
                    Collection = db.CollectionSet.Find(4),
                    LanguageCode = "pt",
                    Title = "Fundo B",
                    Description = "Descri��o do Fundo B.",
                    Provenience = "Proveni�ncia do Fundo B.",
                    AdministrativeAndBiographicStory = "Hist�ria Administrativa e Biogr�fica do Fundo B.",
                    Dimension = "Dimens�o do Fundo B.",
                    FieldAndContents = "�mbito e conte�do do Fundo B.",
                    CopyrightInfo = "Condi��es de Reprodu��o do Fundo B."
                }
            );

            db.SaveChanges();
        }



        protected void SeedCollaborators(ArquivoSilvaMagalhaes.Models.ArchiveDataContext db)
        {
            var collaborators = new List<Collaborator>{
                    new Collaborator{ Id=1, Name="Abc",  Contact="999999999", EmailAddress="abc@mail.com", Task="xyz", ContactVisible=true},
                    new Collaborator{ Id=2, Name="Def", Contact="911111111", EmailAddress="def@mail.com", Task="ghj", ContactVisible=true }
                    
            };
            db.SaveChanges();
        }
    }
}
