using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace gym.Models;

public partial class Userr
{
    public decimal UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? PicPath { get; set; }

    [NotMapped]
    public virtual IFormFile? ImageFile { get; set; }

    public string? Address { get; set; }

    public decimal RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Aboutuspage> Aboutuspages { get; set; } = new List<Aboutuspage>();

    public virtual ICollection<Contactuspage> Contactuspages { get; set; } = new List<Contactuspage>();

    public virtual ICollection<Homepage> Homepages { get; set; } = new List<Homepage>();

    public virtual Rolee Role { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Session> SessionMembers { get; set; } = new List<Session>();

    public virtual ICollection<Session> SessionTrainers { get; set; } = new List<Session>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    public virtual ICollection<WorkoutPlan> WorkoutPlanMembers { get; set; } = new List<WorkoutPlan>();

    public virtual ICollection<WorkoutPlan> WorkoutPlanTrainers { get; set; } = new List<WorkoutPlan>();
}
