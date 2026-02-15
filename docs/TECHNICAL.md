# Technical Overview Document

## Tech Stack with Explanations
- **Frontend**: Blazor WASM
  - Newish frontend that is easy to integrate with ASP.NET
* **Backend**: ASP.NET backend API with Minimal API
  * .NET technologies are the foundation of this project (I wanted to gain proficiency with them) so this is the logical choice
* **Database**: SQLite using EntityFramework in ASP.NET
  * Since the target of this project is self-hosters and the database is not expected to be very complicated nor large, a simple file-based database made the most sense. Docker volume portability was a factor.
* **Deployment**: Docker Compose
  * Pretty easy choice to make cross-platform deployment and consistent deployment environments possible

## High-Level Architecture
This is a simple diagram that explains how different elements in the tech stack work together. You can find it at `./docs/WardrobeManager-Architecture-Diagram.drawio`

## Key Technical Decisions
1. **SQLite Database**
  * Works with EF Core
  * Easily backed up
  * Works well with Docker without needing another container
  * Performance needs of project fit what this database can offer
  
2. **File-base image storage**
  * Simplest way to store user-submitted media
  * Works well with Docker
  * Easy to backup files
  * Easy to view files without specialized software
  * No overhead (besides I/O) when retrieving and storing them (in comparison to storing them in the database and maybe having to convert to Base64)

3. **Authentication Approach**
* Simplest approach to implement
* JWT tokens can work with non-browser based clients (mobile apps)

4. **Version Synchronization**
  * For simplicity, frontend and backend versions will exclusively work with each other if and only if they have the same version.
  * Because the typical deployment method is docker, this should be very simple to setup

5. **Logging**
* A powerful logging system will be implemented to log all important information (exceptions, important events, etc)
* OpenTelemetry will be implemented

6. **Code Design Principles**
   * General 
      * Validation: business logic validation, occurs on client, server, and database
      * Unit Tests: test all individual pieces of logic
      * Integration Tests: test how different components of the project interact
      * `WardrobeManager.Shared`: contains shared models, enums, classes, etc
  * Frontend
    *  ViewModels: separate business logic from views and make them easily testable
  * Backend
    * Repository Pattern: organized and structured access to database
    * Service Pattern: clear separation of business logic
    * Minimal Controllers: clear separation of controller from business logic
    * DTO Mappings: hiding unncessary domain model properties

## Data Models
TODO: will be completed (generated automatically) once development is complete

## API Design
TODO: will be completed (generated automatically with swagger) once development is done

## Testing
* Unit tests - On all logic in frontend and backend
  * Frameworks: NUnit with FluentAssertions
* Integration tests - not sure what the scope will be
* End-To-End tests - most common user pathways (hotpaths)
* Minimum Coverage Necessary: 80% on any new code

## Development Rules
* CI/CD Pipline
  * DeepSource will be used to guard for, code coverage, security issues, maintainability issues
  * Github Actions will be used to build the code on every pull request
  
### Pull Request Process
  * Merging Blockers
      * DeepSource reports under 80% coverage on new code
      * Github Actions are unable to build code
  * Acceptance Criteria of User Story will be reviewed
  * Universal Acceptance Criteria will be reviewed

## Planning Practices
* User stories are the main method of tracking work to be done and will guide sprints
  * Ticket Format: `US-XX` - where XX is the user story’s number
* Tasks are units of work that arise when working on a user story. If they are simple they may be completed without the same user story ticket. Otherwise, a new ticket will be created and be placed on the backlog.
  * Ticket Format: `TS-XXX` where XXX is the task number
