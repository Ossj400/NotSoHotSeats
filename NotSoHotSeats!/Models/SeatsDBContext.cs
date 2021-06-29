using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


#nullable disable

namespace NotSoHotSeats_.Models
{
    public partial class SeatsDBContext : DbContext
    {
        public SeatsDBContext()
        {
        }

        public SeatsDBContext(DbContextOptions<SeatsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Calendarium> Calendaria { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Calendarium>(entity =>
            {
                entity.HasKey(e => e.IdReservation)
                    .HasName("PK_IdReservation")
                    .IsClustered(true);

                entity.ToTable("Calendarium");

                entity.Property(e => e.DateOfReservation).HasColumnType("datetime");

                entity.Property(e => e.IdUser)
                    .IsRequired();

                entity.Property(e => e.SeatSymbol)
                    .IsRequired();
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(e => e.IdSeat)
                    .HasName("PK_IdSeat");

                entity.ToTable("Seat");

                entity.Property(e => e.SeatSymbol)
                    .IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .IsClustered(true);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired();
            });

           // modelBuilder.Ignore<UserLogged>();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        public bool IsSeatTaken(Seat seat, DateTime pickedDate)
        {
            var calendary = GetCalendariumForThisSeat(seat);
            var isTaken = calendary.Where(s => s.DateOfReservation.Date == pickedDate.Date).FirstOrDefault();
            if ( isTaken != null)
               return true;

            return false;
        }

        public string CreateReservation(int userId, DateTime pickedDate, Seat seat)
        {
            if (!IsSeatTaken(seat, pickedDate))
            {
                Calendarium reservation = new Calendarium
                {
                    DateOfReservation = pickedDate.Date,
                    SeatSymbol = seat.SeatSymbol,
                    IdUser = userId
                };
                Calendaria.Add(reservation);
                SaveChanges();
                return "Zarezerwowano";
            }

            var userForSeat = GetUserFromSeat(seat, pickedDate);
            if (userForSeat.IdUser == UserLogged.IdUser)
            {
                CancelReservation(seat, pickedDate);
                return "Anulowano rezerwację";

            }

            else
                throw new Exception("Miejsce jest już zajęte");
        }
        
        public IEnumerable<Calendarium> GetCalendariumForThisSeat(Seat seat)
        {
            return Calendaria.Where(s => s.SeatSymbol == seat.SeatSymbol);
        }

        public Seat GetSeat(string seatSymbol)
        {
            return Seats.Where(s => s.SeatSymbol == seatSymbol).FirstOrDefault();
        }

        public User RegisterUser(string firstName, string lastName, string login, string password)
        {
            if (firstName == "" || lastName == "" || login == "" || password == "")
                throw new Exception("Wypełnij wszystkie pola!");

            var users = Users.Where(u => u.FirstName == firstName && u.LastName == lastName);
            if (users.Any())
                throw new Exception("Użytkownik o takim imieniu i nazwisku już istnieje.\nZmień imię lub nazwisko w urzędzie i spróbój ponownie.");
            users = Users.Where(u => u.Login == login);
            if (users.Any())
                throw new Exception("Użytkownik o takim loginie już istnieje.");

            IPasswordHasher hasher = new PasswordHasher();
            string hashedPassword = hasher.HashPassword(password);
            User user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Login = login,
                Password = hashedPassword
            };
            Users.Add(user);
            SaveChanges();
            return user;
        }
        public bool AddLoggedUser(int userId)
        {
            var user = Users.Where(u => u.IdUser == userId).FirstOrDefault();
            UserLogged.FirstName = user.FirstName;
            UserLogged.LastName = user.LastName;
            UserLogged.IdUser = userId;
            return true;
        }

        public User Login(string login, string password)
        {
            IPasswordHasher hasher = new PasswordHasher();

            User user = Users.Where(u => u.Login == login).FirstOrDefault();
            if(user != null && hasher.VerifyHashedPassword(user.Password, password) == Microsoft.AspNet.Identity.PasswordVerificationResult.Success)
                return user;

            return null;
        }

        public User GetUserFromSeat(Seat seat, DateTime pickedDate)
        {
            var seatCalendarium = GetCalendariumForThisSeat(seat);
            var takenBy = seatCalendarium.Where(u => u.DateOfReservation.Date == pickedDate.Date).Select(us=>us.IdUser).FirstOrDefault();
            var user = Users.Where(u => u.IdUser == takenBy).FirstOrDefault();
            return user;
        }

        public void CancelReservation(Seat seat, DateTime pickedDate)
        {
            var calendariumForSeat = GetCalendariumForThisSeat(seat);
            var reservationToDelete = calendariumForSeat.Where(c => c.SeatSymbol == seat.SeatSymbol
                && c.DateOfReservation.Date == pickedDate.Date && c.IdUser == UserLogged.IdUser).FirstOrDefault();
            Calendaria.Remove(reservationToDelete);
            SaveChanges();
        }

    }
}
