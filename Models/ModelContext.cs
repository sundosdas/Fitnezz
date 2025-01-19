using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace gym.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutuspage> Aboutuspages { get; set; }

    public virtual DbSet<Contactusform> Contactusforms { get; set; }

    public virtual DbSet<Contactuspage> Contactuspages { get; set; }

    public virtual DbSet<Homepage> Homepages { get; set; }

    public virtual DbSet<MembershipPlan> MembershipPlans { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Rolee> Rolees { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<StaticPage> StaticPages { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<SubscriptionWorkout> SubscriptionWorkouts { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<Userr> Userrs { get; set; }

    public virtual DbSet<WorkoutPlan> WorkoutPlans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=localhost:1521;USER ID=C##SUNDOS;PASSWORD=806588;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##SUNDOS")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutuspage>(entity =>
        {
            entity.HasKey(e => e.AbModificationid).HasName("SYS_C008526");

            entity.ToTable("ABOUTUSPAGE");

            entity.Property(e => e.AbModificationid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("AB_MODIFICATIONID");
            entity.Property(e => e.Adminid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ADMINID");
            entity.Property(e => e.Headertitle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HEADERTITLE");
            entity.Property(e => e.Image1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGE1");
            entity.Property(e => e.Image2)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGE2");
            entity.Property(e => e.Lastupdated)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("LASTUPDATED");
            entity.Property(e => e.Paragraph1)
                .IsUnicode(false)
                .HasColumnName("PARAGRAPH1");
            entity.Property(e => e.Paragraph2)
                .IsUnicode(false)
                .HasColumnName("PARAGRAPH2");
            entity.Property(e => e.Paragraph3)
                .IsUnicode(false)
                .HasColumnName("PARAGRAPH3");

            entity.HasOne(d => d.Admin).WithMany(p => p.Aboutuspages)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("FK_ABOUTUSPAGE_USER");
        });

        modelBuilder.Entity<Contactusform>(entity =>
        {
            entity.HasKey(e => e.Formid).HasName("SYS_C008540");

            entity.ToTable("CONTACTUSFORM");

            entity.Property(e => e.Formid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("FORMID");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Isreviewed)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'N' ")
                .IsFixedLength()
                .HasColumnName("ISREVIEWED");
            entity.Property(e => e.Message)
                .HasColumnType("CLOB")
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONE");
            entity.Property(e => e.Submittedat)
                .HasPrecision(6)
                .HasDefaultValueSql("SYSTIMESTAMP")
                .HasColumnName("SUBMITTEDAT");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
        });

        modelBuilder.Entity<Contactuspage>(entity =>
        {
            entity.HasKey(e => e.CModificationid).HasName("SYS_C008510");

            entity.ToTable("CONTACTUSPAGE");

            entity.Property(e => e.CModificationid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("C_MODIFICATIONID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Adminid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ADMINID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Lastupdated)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("LASTUPDATED");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONE");

            entity.HasOne(d => d.Admin).WithMany(p => p.Contactuspages)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("FK_CONTACTUSPAGE_USER");
        });

        modelBuilder.Entity<Homepage>(entity =>
        {
            entity.HasKey(e => e.HModificationid).HasName("SYS_C008532");

            entity.ToTable("HOMEPAGE");

            entity.Property(e => e.HModificationid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("H_MODIFICATIONID");
            entity.Property(e => e.Adminid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ADMINID");
            entity.Property(e => e.Lastupdated)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("LASTUPDATED");
            entity.Property(e => e.Sliderimage1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SLIDERIMAGE1");
            entity.Property(e => e.Sliderimage2)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SLIDERIMAGE2");
            entity.Property(e => e.Slidersubtitle1)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("SLIDERSUBTITLE1");
            entity.Property(e => e.Slidersubtitle2)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("SLIDERSUBTITLE2");
            entity.Property(e => e.Slidertitle1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SLIDERTITLE1");
            entity.Property(e => e.Slidertitle2)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SLIDERTITLE2");

            entity.HasOne(d => d.Admin).WithMany(p => p.Homepages)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("FK_HOMEPAGE_USER");
        });

        modelBuilder.Entity<MembershipPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("SYS_C008445");

            entity.ToTable("MEMBERSHIP_PLANS");

            entity.Property(e => e.PlanId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PLAN_ID");
            entity.Property(e => e.Details)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DETAILS");
            entity.Property(e => e.PDuration)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("P_DURATION");
            entity.Property(e => e.PlanName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("PLAN_NAME");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("PRICE");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("SYS_C008501");

            entity.ToTable("REPORTS");

            entity.Property(e => e.ReportId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("REPORT_ID");
            entity.Property(e => e.PdfPath)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("PDF_PATH");
            entity.Property(e => e.ReportTitle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("REPORT_TITLE");
            entity.Property(e => e.ReportType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("REPORT_TYPE");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("SYS_C008483");

            entity.ToTable("REVIEWS");

            entity.Property(e => e.ReviewId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("REVIEW_ID");
            entity.Property(e => e.FeedbackText)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("FEEDBACK_TEXT");
            entity.Property(e => e.Rating)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RATING");
            entity.Property(e => e.SessionId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SESSION_ID");
            entity.Property(e => e.SubmittedAt)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("SUBMITTED_AT");

            entity.HasOne(d => d.Session).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008484");
        });

        modelBuilder.Entity<Rolee>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("SYS_C008430");

            entity.ToTable("ROLEES");

            entity.HasIndex(e => e.RoleName, "SYS_C008431").IsUnique();

            entity.Property(e => e.RoleId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLE_NAME");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("SYS_C008470");

            entity.ToTable("SCHEDULES");

            entity.Property(e => e.ScheduleId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SCHEDULE_ID");
            entity.Property(e => e.AvailableFrom)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("AVAILABLE_FROM");
            entity.Property(e => e.AvailableTo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("AVAILABLE_TO");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DAY_OF_WEEK");
            entity.Property(e => e.SessionId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SESSION_ID");
            entity.Property(e => e.TrainerId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TRAINER_ID");

            entity.HasOne(d => d.Session).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008472");

            entity.HasOne(d => d.Trainer).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008471");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("SYS_C008461");

            entity.ToTable("SESSIONS");

            entity.Property(e => e.SessionId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SESSION_ID");
            entity.Property(e => e.CheckIn)
                .HasPrecision(6)
                .HasColumnName("CHECK_IN");
            entity.Property(e => e.CheckOut)
                .HasPrecision(6)
                .HasColumnName("CHECK_OUT");
            entity.Property(e => e.EndTime)
                .HasPrecision(6)
                .HasColumnName("END_TIME");
            entity.Property(e => e.MemberId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MEMBER_ID");
            entity.Property(e => e.StartTime)
                .HasPrecision(6)
                .HasColumnName("START_TIME");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.TrainerId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TRAINER_ID");

            entity.HasOne(d => d.Member).WithMany(p => p.SessionMembers)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008462");

            entity.HasOne(d => d.Trainer).WithMany(p => p.SessionTrainers)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008463");
        });

        modelBuilder.Entity<StaticPage>(entity =>
        {
            entity.HasKey(e => e.PageId).HasName("SYS_C008493");

            entity.ToTable("STATIC_PAGES");

            entity.Property(e => e.PageId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PAGE_ID");
            entity.Property(e => e.LastUpdated)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP\n")
                .HasColumnName("LAST_UPDATED");
            entity.Property(e => e.PageContent)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PAGE_CONTENT");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("SYS_C008452");

            entity.ToTable("SUBSCRIPTIONS");

            entity.Property(e => e.SubscriptionId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SUBSCRIPTION_ID");
            entity.Property(e => e.EndDate)
                .HasColumnType("DATE")
                .HasColumnName("END_DATE");
            entity.Property(e => e.InvoicePath)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("INVOICE_PATH");
            entity.Property(e => e.PlanId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PLAN_ID");
            entity.Property(e => e.StartDate)
                .HasColumnType("DATE")
                .HasColumnName("START_DATE");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Plan).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008454");

            entity.HasOne(d => d.User).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008453");
        });

        modelBuilder.Entity<SubscriptionWorkout>(entity =>
        {
            entity.HasKey(e => new { e.SubscriptionId, e.WorkoutId })
                .HasName("SYS_C_SUBSCRIPTION_WORKOUT_PK");

            entity.ToTable("SUBSCRIPTION_WORKOUTS");

            entity.Property(e => e.SubscriptionId)
                .HasColumnType("NUMBER(38)") // Matches the database type
                .HasColumnName("SUBSCRIPTION_ID");

            entity.Property(e => e.WorkoutId)
                .HasColumnType("NUMBER(38)") // Matches the database type
                .HasColumnName("WORKOUT_ID");

            entity.HasOne(d => d.Subscription)
                .WithMany(p => p.SubscriptionWorkouts)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SUBSCRIPTION_WORKOUT_SUBSCRIPTION");

            entity.HasOne(d => d.Workout)
                .WithMany(p => p.SubscriptionWorkouts)
                .HasForeignKey(d => d.WorkoutId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SUBSCRIPTION_WORKOUT_WORKOUT");
        });


        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.TestimonialId).HasName("SYS_C008488");

            entity.ToTable("TESTIMONIALS");

            entity.Property(e => e.TestimonialId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TESTIMONIAL_ID");
            entity.Property(e => e.IsApproved)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'Pending'")
                .HasColumnName("IS_APPROVED");
            entity.Property(e => e.SubmittedAt)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("SUBMITTED_AT");
            entity.Property(e => e.TContent)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("T_CONTENT");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008489");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.LoginId).HasName("SYS_C008497");

            entity.ToTable("USER_LOGIN");

            entity.HasIndex(e => e.Username, "SYS_C008498").IsUnique();

            entity.Property(e => e.LoginId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("LOGIN_ID");
            entity.Property(e => e.Pass)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASS");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("SYS_C008499");
        });

        modelBuilder.Entity<Userr>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("SYS_C008437");

            entity.ToTable("USERRS");

            entity.HasIndex(e => e.Email, "SYS_C008438").IsUnique();

            entity.HasIndex(e => e.Phone, "SYS_C008439").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Address)
                .HasMaxLength(600)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PHONE");
            entity.Property(e => e.PicPath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("PIC_PATH");
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLE_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.Userrs)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008440");
        });

        modelBuilder.Entity<WorkoutPlan>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("SYS_C008476");

            entity.ToTable("WORKOUT_PLANS");

            entity.Property(e => e.WorkoutId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("WORKOUT_ID");
            entity.Property(e => e.Details)
                .HasColumnType("CLOB")
                .HasColumnName("DETAILS");
            entity.Property(e => e.Goals)
                .HasColumnType("CLOB")
                .HasColumnName("GOALS");
            entity.Property(e => e.MemberId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MEMBER_ID");
            entity.Property(e => e.TrainerId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TRAINER_ID");

            entity.HasOne(d => d.Member).WithMany(p => p.WorkoutPlanMembers)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008478");

            entity.HasOne(d => d.Trainer).WithMany(p => p.WorkoutPlanTrainers)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008477");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
