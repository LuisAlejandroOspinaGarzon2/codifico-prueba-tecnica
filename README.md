# Codifico Prueba Técnica

## Descripción

Este proyecto es parte de una prueba técnica para la posición de desarrollador en Codifico. La aplicación desarrollada, "Sales Date Prediction," tiene como objetivo permitir la creación de órdenes y predecir cuándo ocurrirá la próxima orden por cliente, utilizando registros almacenados en una base de datos.

## Tecnologías

- .NET Core 8.0.401
- Angular 18.2.0
- SQL Server 2019 Express
- D3.js 7.9.0

## Requisitos Previos

- Visual Studio Code o cualquier editor de texto.
- Node.js v20.16.0 y npm 10.8.2.
- SQL Server Management Studio 20.2.
- Instancia de SQL Server.

## Configuración del Proyecto

### Backend

1. Clona este repositorio.
2. Navega al directorio `CodificoWebAPI`.
3. Ejecuta `dotnet build` para compilar la solución.
4. Configura la cadena de conexión en el archivo `appsettings.json`.
5. Ejecuta `dotnet run` para iniciar el servidor backend en `http://localhost:5011`.
6. La API está configurada para manejar solicitudes desde el frontend en Angular.

### Frontend

1. Navega al directorio `codifico-webapp`.
2. Ejecuta `npm install` para instalar todas las dependencias necesarias.
3. Ejecuta `npm start` para iniciar la aplicación Angular. Esta se servirá en `http://localhost:4200`.
4. Las rutas principales de la aplicación son:
   - `/` - Vista principal de predicción de ventas (Sales Date Prediction View).
   - `/orders` - Vista de las órdenes del cliente seleccionado (Orders View).
   - `/new-order` - Formulario para crear una nueva orden (New Order Form).

### D3.js Application

1. Navega al directorio `d3-bar-chart`.
2. Abre el archivo `index.html` en un navegador web para visualizar la aplicación.
3. Ingrese una lista de números enteros separados por comas en la caja de texto "Source Data".
4. Haz clic en "Update Data" para generar el gráfico de barras.

## Pruebas Unitarias

Este proyecto incluye pruebas unitarias para todos los controladores de la Web API. A continuación se detalla cada una de las pruebas implementadas:

### Controladores y Pruebas

#### 1. CustomersController

- **GetCustomerOrderPredictions_ReturnsPredictions**: Verifica que se generen correctamente las predicciones de la próxima orden basada en órdenes anteriores.
- **GetOrdersByCustomer_ReturnsOrders**: Verifica que se obtengan correctamente las órdenes de un cliente específico.
- **CreateCustomer_AddsNewCustomer**: Verifica que se agregue un nuevo cliente a la base de datos.
- **UpdateCustomer_UpdatesExistingCustomer**: Verifica que se actualice correctamente un cliente existente.
- **DeleteCustomer_RemovesCustomer**: Verifica que se elimine correctamente un cliente de la base de datos.

#### 2. OrdersController

- **GetOrders_ReturnsAllOrders**: Verifica que se obtengan todas las órdenes.
- **GetOrder_ReturnsOrderById**: Verifica que se obtenga una orden específica por su ID.
- **CreateOrder_AddsNewOrder**: Verifica que se cree y se guarde una nueva orden en la base de datos.

#### 3. ProductsController

- **GetProducts_ReturnsAllProducts**: Verifica que se obtengan todos los productos.
- **GetProductById_ReturnsProduct**: Verifica que se obtenga un producto específico por su ID.

#### 4. EmployeesController

- **GetEmployees_ReturnsAllEmployees**: Verifica que se obtengan todos los empleados con sus nombres completos.

#### 5. ShippersController

- **GetShippers_ReturnsAllShippers**: Verifica que se obtengan todos los transportistas (shippers).

#### Base de datos

- Se configuró y se ejecutó el script `DBSetup.sql` para crear la base de datos `StoreSample`, que incluye esquemas, tablas, índices, y vistas necesarias.
- Se desarrollaron las consultas DML para obtener predicciones de la próxima orden, listar órdenes por cliente, obtener empleados, transportistas, y productos, así como para agregar nuevas órdenes y sus detalles.

#### Web API

Se creó un proyecto Web API con .NET Core que expone los siguientes servicios REST a través de controladores:

- Listar clientes con fecha de última orden y fecha de posible orden (`SalesPredictionController`).
- Listar órdenes por cliente (`OrdersController`).
- Listar todos los empleados (`EmployeesController`).
- Listar todos los transportistas (`ShippersController`).
- Listar todos los productos (`ProductsController`).
- Crear una orden nueva con un producto (`OrdersController`).


#### Web App

Se construyó un SPA en Angular 18.2.0 que permite:

- **Sales Date Prediction View**: Configurada como el home de la aplicación, con la lista de clientes, paginación, ordenamiento, y botones para ver órdenes y crear nuevas órdenes.
- **Orders View**: Vista que muestra una lista de órdenes del cliente seleccionado, con paginación y ordenamiento.
- **New Order Form**: Formulario para crear una nueva orden con validación de datos.
