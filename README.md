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
:begin
// Step 1: Create a Unique Constraint
CREATE CONSTRAINT UNIQUE_IMPORT_NAME 
FOR (node:`UNIQUE IMPORT LABEL`) 
REQUIRE (node.`UNIQUE IMPORT ID`) IS UNIQUE;
:commit

// Step 2: Wait for Index Creation
CALL db.awaitIndexes(300);

// Step 3: Import Nodes
:begin
UNWIND [
  {_id:0, properties:{name:"MainGate"}},
  {_id:1, properties:{name:"Stationery"}},
  {_id:2, properties:{name:"WorkShop"}},
  {_id:3, properties:{name:"Parking"}},
  {_id:4, properties:{name:"VolleyballCourt"}},
  {_id:5, properties:{name:"BasketballCourt"}},
  {_id:7, properties:{name:"SoilLab"}},
  {_id:8, properties:{name:"FootballGround"}},
  {_id:9, properties:{name:"Canteen"}},
  {_id:10, properties:{name:"MainBuilding"}},
  {_id:12, properties:{name:"GroundToilet"}},
  {_id:13, properties:{name:"Garden"}},
  {_id:14, properties:{name:"SecondBuilding"}},
  {_id:15, properties:{name:"WaterTap2"}},
  {_id:16, properties:{name:"BadmintonCourt"}}
] AS row
CREATE (n:`UNIQUE IMPORT LABEL`{`UNIQUE IMPORT ID`: row._id}) 
SET n += row.properties 
SET n:Location;
:commit

// Step 4: Create Relationships
:begin
UNWIND [
  {start: {_id:0}, end: {_id:1}, properties:{instruction:"Walk straight,look left", weight:1}},
  {start: {_id:0}, end: {_id:3}, properties:{instruction:"turn right move ahead and turn right", weight:5}},
  {start: {_id:0}, end: {_id:4}, properties:{instruction:"turn right move ahead,turn right", weight:3}},
  {start: {_id:0}, end: {_id:5}, properties:{instruction:"turn right move ahead", weight:2}},
  {start: {_id:0}, end: {_id:8}, properties:{instruction:"walk straight, look right", weight:4}},
  {start: {_id:1}, end: {_id:0}, properties:{instruction:"Walk straight", weight:1}},
  {start: {_id:1}, end: {_id:2}, properties:{instruction:"Walk straight,look left", weight:2}},
  {start: {_id:2}, end: {_id:1}, properties:{instruction:"Walk straight,look right", weight:2}},
  {start: {_id:2}, end: {_id:16}, properties:{instruction:"walk straight, look left", weight:3}},
  {start: {_id:3}, end: {_id:0}, properties:{instruction:"move towards basketballcourt, turnleft move ahead,", weight:5}},
  {start: {_id:4}, end: {_id:0}, properties:{instruction:"move towards basketballcourt and rotate until you see gate", weight:3}},
  {start: {_id:5}, end: {_id:0}, properties:{instruction:"rotate until you see gate", weight:2}},
  {start: {_id:7}, end: {_id:9}, properties:{instruction:"move straight and look right", weight:4}},
  {start: {_id:8}, end: {_id:0}, properties:{instruction:"rotate around until you see gate , move toward it", weight:4}},
  {start: {_id:9}, end: {_id:7}, properties:{instruction:"follow foothpath opposite of big buiding", weight:4}},
  {start: {_id:9}, end: {_id:10}, properties:{instruction:"move toward big building ", weight:4}},
  {start: {_id:10}, end: {_id:9}, properties:{instruction:"move towards left footpath until you reach canteen ", weight:4}},
  {start: {_id:10}, end: {_id:12}, properties:{instruction:"turn left go ahead , take next left and move straight until you find toilet", weight:15}},
  {start: {_id:10}, end: {_id:15}, properties:{instruction:"turn right and move straight until you see tap", weight:7}},
  {start: {_id:12}, end: {_id:10}, properties:{instruction:"rotate around and take the footpath which is present in left of mainBuilding, take left turn at the end of path", weight:15}},
  {start: {_id:13}, end: {_id:15}, properties:{instruction:"move towards FootballGround and turn right when footpath arrives", weight:4}},
  {start: {_id:14}, end: {_id:15}, properties:{instruction:"move ahead until tap arrives in rightside", weight:1}},
  {start: {_id:15}, end: {_id:10}, properties:{instruction:"rotate around and move towards the big building", weight:7}},
  {start: {_id:15}, end: {_id:13}, properties:{instruction:"move towards big building take first left turn", weight:4}},
  {start: {_id:15}, end: {_id:14}, properties:{instruction:"building just front is 2ndBuilding", weight:1}},
  {start: {_id:15}, end: {_id:16}, properties:{instruction:"rotate until you find court", weight:1}},
  {start: {_id:16}, end: {_id:2}, properties:{instruction:"move opposite of watertap towards gate until you see a door in rightside", weight:3}},
  {start: {_id:16}, end: {_id:15}, properties:{instruction:"Rotate around until you see tap", weight:1}}
] AS row
MATCH (start:`UNIQUE IMPORT LABEL`{`UNIQUE IMPORT ID`: row.start._id})
MATCH (end:`UNIQUE IMPORT LABEL`{`UNIQUE IMPORT ID`: row.end._id})
CREATE (start)-[r:CONNECTED_TO]->(end) 
SET r += row.properties;
:commit

// Step 5: Remove Temporary Labels
:begin
MATCH (n:`UNIQUE IMPORT LABEL`)  
WITH n LIMIT 20000 
REMOVE n:`UNIQUE IMPORT LABEL` 
REMOVE n.`UNIQUE IMPORT ID`;
:commit

// Step 6: Drop the Constraint
:begin
DROP CONSTRAINT UNIQUE_IMPORT_NAME;
:commit

````
