# LecMap

LecMap is a navigation API designed to provide optimal paths between various locations in my college. Built as a fun project, it allows users to query paths interactively via Swagger UI. The application leverages graph-based algorithms and integrates **Llama3 70B from Groq** to understand the query and produce human-readable navigation instructions.
## Features

- 🚩 Get the shortest path between any two areas in the college.
- 🧭 Human-readable directions powered by **Llama3 70B**.
- 📍 Interact seamlessly using the **Swagger UI**.
- 🏫 Designed specifically for a college setup.
- 🗺️ Utilizes Neo4j to store and query graph-based data.

## Technologies Used

- **.NET Core**: Backend API development.
- **Swagger UI**: For interactive API exploration.
- **Graph-Based Algorithms**: Used for determining optimal paths.
- **Neo4j**: For storing and querying campus navigation data.
- **Llama3 70B (Groq)**: Analyzes the query, identifies the start and end nodes, and generates natural language navigation instructions.

## Setup Instructions

To set up and run LecMap locally, follow these steps:
### Prerequisites
Ensure you have the following installed and configured:

- **.NET 8 SDK**
- **Visual Studio** or **Visual Studio Code**
- **Neo4j Database** (local or cloud instance)
- **Groq Account** (for API access to **Llama3 70B**)


---

### Steps to Run the Application

#### **1. Clone the Repository**
```bash
git clone https://github.com/Shirish-Bashyal/lecmap.git
```

#### **2. Set Up the Neo4j Database**
- Open Neo4j Desktop or connect to your Neo4j instance.
- Run the following Cypher query in the Neo4j Browser to set up the database
- Update the `appsettings.json` file in the project.

#### **3. Configure the Groq Llama3 70B Integration**
- Obtain your API key from Groq.
- Update the `appsettings.json` file in the project.

 #### **4.Run the Application**


 #### **Cypher query**
 ````bash
query
````
