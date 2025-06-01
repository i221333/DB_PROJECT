# DB_PROJECT
Flex Trainer: Automating Gym Management and Fitness Tracking.

## Project Overview
Flex Trainer is a comprehensive management system designed for gyms and health membership organizations. It streamlines operations for gym owners, trainers, and members by automating record-keeping, fitness planning, diet management, and progress tracking. The system provides tailored interfaces for each user role—members, trainers, gym owners, and admins—ensuring user-friendly access and efficient management of health and fitness data.

## Features

### Member 
- **Sign Up & Login**: Secure registration and account access.
- **Personalized Workouts**: Create, view, and filter workout plans by goal, schedule, and trainer.
- **Diet Plans**: Build or select diet plans, filter by type, nutrition, allergens, and trainer.
- **Session Booking**: Book personal training sessions.
- **Progress Tracking**: Log workouts, diet, calories, and daily targets.
- **Trainer Feedback**: Rate and review trainers.

### Trainer
- **Sign Up & Login**: Register (with gym selection and owner verification) or log in.
- **Appointment Management**: View, schedule, reschedule, or cancel client sessions.
- **Workout & Diet Planning**: Create and assign plans to clients.
- **Client Reports**: Track client progress, filter and generate workout/diet reports.
- **Feedback Overview**: View ratings and reviews received from clients across gyms.

### Gym Owner
- **Sign Up & Approval**: Register gym (admin approval required).
- **Login**: Secure admin access.
- **Member & Trainer Reports**: View/filter member and trainer details, generate reports.
- **Add Trainers**: Register new trainers.
- **Account Management**: Remove member or trainer accounts.

### Admin
- **Login**: Secure access for admin operations.
- **Gym Performance Reports**: View metrics for all gyms.
- **Gym Registration Approval**: Approve/reject new gym registrations.
- **Revoke Gyms**: Remove gyms from the system.

## Project Structure
```text
DB_PROJECT/
├───gym-management-system/
|   ├───FlexTrainer-database.sql
|   ├───data/                                  # CSV files for database population
|   ├───images/                                # Images used in application interfaces
|   └───member_forms/                          # Forms (design + backend code) catered to each type of user
└───README.md   
```

## Prerequisites
**Visual Studio** (2022)  
- Modify via Visual Studio Installer to include **.NET Desktop Development**  

**Microsoft SQL Server Management Studio** (2019)
   
## Getting Started
#### 1. Clone the repository  
   - Clone and open the project folder in Visual Studio.

#### 2. Set up the backend  
   - Setup a database and run the SQL commands in `FlexTrainer-database.sql`.
   - Use the import feature to populate your database.
   - Configure the database and update connection strings in the backend code.
     
#### 3. Run the application  
   - Run the `Program.cs` file.

