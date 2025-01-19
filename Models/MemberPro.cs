namespace gym.Models
{ 
    public class MemberPro
    {
        public Userr User { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<WorkoutPlan> Workouts { get; set; }
        public List<Session> Sessions { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
