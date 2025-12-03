# Notes-Api

## Features
- Create, read, update, and delete (soft delete) notes
- Archive / unarchive notes
- Search notes by title or content
- Pagination support on list endpoints
- Data persistence using SQLite + Entity Framework Core
- Layered architecture:
  - **Controller** → HTTP routing & DTO mapping
  - **Service** → business logic
  - **DbContext** → data persistence
- Fully async API for scalable IO operations
- Swagger UI for easy endpoint testing

## Technologies Used
- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQLite** database
- **Swagger / Swashbuckle**

## Endpoints

 Method | Route                     | Description                |
|-------:|---------------------------|----------------------------|
| GET    | `/api/notes`              | List notes (search + paging) |
| GET    | `/api/notes/{id}`         | Retrieve a single note     |
| POST   | `/api/notes`              | Create a new note          |
| PUT    | `/api/notes/{id}`         | Update an existing note    |
| DELETE | `/api/notes/{id}`         | Soft delete a note         |
| PATCH  | `/api/notes/{id}/archive` | Toggle archive state       |
| GET    | `/api/notes/hello`        | Test endpoint              |

### Query params on GET `/api/notes`

| Param        | Type   | Default | Purpose                     |
|--------------|--------|---------|-----------------------------|
| `page`       | int    | 1       | Page number                 |
| `pageSize`   | int    | 10      | Number of results per page  |
| `showArchived` | bool | false   | Include archived notes      |
| `searchTerm` | string | null    | Filter by content / title   |
