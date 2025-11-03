## Setup Backend ##
dotnet new webapi -n EmployeeApi
cd EmployeeApi

dotnet add package Microsoft.Data.SqlClient

## Setup Frontend ##
ng new employee-frontend --standalone --routing=false --style=css
cd employee-frontend

npm install bootstrap
