using HomeZone.Services.SavingsAPI.Data;
using HomeZone.Services.SavingsAPI.Model;
using HomeZone.Services.SavingsAPI.Model.Dto;
using HomeZone.Services.SavingsAPI.Services.IServices;

namespace HomeZone.Services.SavingsAPI.Services
{


    // The service class that handles savings plan logic and implements the interface contract
    public class SavingsPlanService : ISavingsPlanService
    {
        // EF Core DbContext used to access the database
        private readonly AppDbContext _context;

        // Constructor that injects the DbContext via dependency injection
        public SavingsPlanService(AppDbContext context)
        {
            _context = context;
        }

        // Method to calculate the savings breakdown before creating the plan
        public async Task<SavingsBreakdownDto> CalculateBreakdownAsync(SavingsPlanRequestDto dto)
        {
            //Add a check

            if (dto.AmountPerPeriod <= 0)
                throw new ArgumentException("Amount per period must be greater than zero.");
            // Calculate how many payments are needed to reach the total savings goal
            //int totalPayments = (int)Math.Ceiling(dto.TotalAmount / dto.AmountPerPeriod);
            int totalPayments = (int)Math.Ceiling(dto.TotalAmount / dto.AmountPerPeriod);

            // Initialize a list to hold each individual savings schedule item (preview only)
            //List<SavingsScheduleDto> schedules = new();
            List<SavingsScheduleDto> schedules = new();

            // Start from the date selected by the user
            DateTime currentDate = dto.StartDate;

            // Loop through the number of payments and generate each schedule entry
            for (int i = 1; i <= totalPayments; i++)
            {
                // Add a new scheduled savings entry to the list
                schedules.Add(new SavingsScheduleDto
                {
                    PaymentNumber = i,              // e.g., 1st payment, 2nd payment, etc.
                    ScheduledDate = currentDate,    // The date this payment is due
                    Amount = dto.AmountPerPeriod    // The amount to be saved for this period
                });

                // Move to the next payment date based on frequency (weekly/monthly)
                currentDate = dto.Frequency.ToLower() switch
                {
                    "weekly" => currentDate.AddDays(7),      // Add 7 days for weekly savings
                    "monthly" => currentDate.AddMonths(1),   // Add 1 month for monthly savings
                    _ => throw new Exception("Unsupported frequency") // Handle unsupported values
                };
            }

            // Return a DTO that contains the full breakdown
            return new SavingsBreakdownDto
            {
                TotalPayments = totalPayments,   // How many payments are in this plan
                Schedule = schedules              // The list of individual scheduled payments
            };
        }

        // Method to persist a new savings plan to the database
        public async Task<Guid> CreateSavingsPlanAsync(SavingsPlanRequestDto dto, string userId)
        {
            //Add a check
            if (dto.AmountPerPeriod <= 0)
                throw new ArgumentException("Amount per period must be greater than zero.");

            // Create a new savings plan entity with all relevant fields
            var plan = new SavingsPlan
            {
                Id = Guid.NewGuid(),                 // Generate a unique ID for the plan
                UserId = userId,                     // Link the plan to the currently logged-in user
                PlanName = dto.PlanName,             // User's chosen name for the plan
                Purpose = dto.Purpose,               // Reason for saving (e.g. rent, house, car)
                AmountPerPeriod = dto.AmountPerPeriod, // Amount to save per cycle
                TotalAmount = dto.TotalAmount,       // Total amount to save
                Frequency = dto.Frequency,           // Frequency of savings (weekly/monthly)
                StartDate = dto.StartDate            // When the plan starts
            };

            // Recalculate how many payment entries are needed
            int totalPayments = (int)Math.Ceiling(dto.TotalAmount / dto.AmountPerPeriod);
            DateTime currentDate = dto.StartDate;

            // Loop through to create each scheduled payment entry
            for (int i = 1; i <= totalPayments; i++)
            {
                // Make sure the Schedules list is initialized (safe check)
                plan.Schedules ??= new List<SavingsSchedule>();

                // Add a new payment schedule to the plan
                plan.Schedules.Add(new SavingsSchedule
                {
                    Id = Guid.NewGuid(),           // Unique ID for this schedule entry
                    PaymentNumber = i,             // Order in the sequence
                    ScheduledDate = currentDate,   // Date the user should save
                    Amount = dto.AmountPerPeriod   // Amount to save for this schedule
                });

                // Update the next payment date based on selected frequency
                currentDate = dto.Frequency.ToLower() switch
                {
                    "weekly" => currentDate.AddDays(7),      // Add 7 days if weekly
                    "monthly" => currentDate.AddMonths(1),   // Add 1 month if monthly
                    _ => throw new Exception("Unsupported frequency") // Handle invalid values
                };
            }

            // Add the full savings plan (with its schedules) to the database
            _context.SavingsPlans.Add(plan);

            // Persist the changes in the database
            await _context.SaveChangesAsync();

            // Return the unique ID of the created savings plan
            return plan.Id;
        }
    }

    //public class SavingsPlanService : ISavingsPlanService
    //{
    //    private readonly AppDbContext _context;

    //    public SavingsPlanService(AppDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<SavingsBreakdownDto> CalculateBreakdownAsync(SavingsPlanRequestDto dto)
    //    {
    //        int totalPayments = (int)Math.Ceiling(dto.TotalAmount / dto.AmountPerPeriod);
    //        List<SavingsScheduleDto> schedules = new();

    //        DateTime currentDate = dto.StartDate;

    //        for (int i = 1; i <= totalPayments; i++)
    //        {
    //            schedules.Add(new SavingsScheduleDto
    //            {
    //                PaymentNumber = i,
    //                ScheduledDate = currentDate,
    //                Amount = dto.AmountPerPeriod
    //            });

    //            currentDate = dto.Frequency.ToLower() switch
    //            {
    //                "weekly" => currentDate.AddDays(7),
    //                "monthly" => currentDate.AddMonths(1),
    //                _ => throw new Exception("Unsupported frequency")
    //            };
    //        }

    //        return new SavingsBreakdownDto
    //        {
    //            TotalPayments = totalPayments,
    //            Schedule = schedules
    //        };
    //    }

    //    public async Task<Guid> CreateSavingsPlanAsync(SavingsPlanRequestDto dto, string userId)
    //    {
    //        var plan = new SavingsPlan
    //        {
    //            Id = Guid.NewGuid(),
    //            UserId = userId,
    //            PlanName = dto.PlanName,
    //            Purpose = dto.Purpose,
    //            AmountPerPeriod = dto.AmountPerPeriod,
    //            TotalAmount = dto.TotalAmount,
    //            Frequency = dto.Frequency,
    //            StartDate = dto.StartDate
    //        };

    //        int totalPayments = (int)Math.Ceiling(dto.TotalAmount / dto.AmountPerPeriod);
    //        DateTime currentDate = dto.StartDate;

    //        for (int i = 1; i <= totalPayments; i++)
    //        {
    //            plan.Schedules ??= new List<SavingsSchedule>();
    //            plan.Schedules.Add(new SavingsSchedule
    //            {
    //                Id = Guid.NewGuid(),
    //                PaymentNumber = i,
    //                ScheduledDate = currentDate,
    //                Amount = dto.AmountPerPeriod
    //            });

    //            currentDate = dto.Frequency.ToLower() switch
    //            {
    //                "weekly" => currentDate.AddDays(7),
    //                "monthly" => currentDate.AddMonths(1),
    //                _ => throw new Exception("Unsupported frequency")
    //            };
    //        }

    //        _context.SavingsPlans.Add(plan);
    //        await _context.SaveChangesAsync();

    //        return plan.Id;
    //    }
    //}

}
