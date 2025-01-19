using gym.Models;

public class SubscriptionWorkout
{
    public decimal SubscriptionId { get; set; }
    public decimal WorkoutId { get; set; }

    // Navigation properties
    public Subscription Subscription { get; set; }
    public WorkoutPlan Workout { get; set; }
}
