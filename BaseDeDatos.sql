CREATE DATABASE SistemaUnidadSalud;
GO

USE SistemaUnidadSalud;
GO

--Tabla de Pacientes
CREATE TABLE Pacientes (
    NumeroExpediente VARCHAR(50) PRIMARY KEY,
    Nombre VARCHAR(150) NOT NULL,
    Edad INT NOT NULL,
    Sexo VARCHAR(20) NOT NULL,
    Direccion VARCHAR(250) NOT NULL,
    Responsable VARCHAR(150) NOT NULL,
    TipoPaciente VARCHAR(50) NOT NULL
);
GO

--Tabla de Consultas
CREATE TABLE Consultas (
    IdConsulta INT IDENTITY(1,1) PRIMARY KEY,
    NumeroExpediente VARCHAR(50) FOREIGN KEY REFERENCES Pacientes(NumeroExpediente),
    FechaConsulta DATETIME DEFAULT GETDATE(),
    ConsultaPor VARCHAR(MAX) NOT NULL,
    PresenteEnfermedad VARCHAR(MAX) NOT NULL,
 
    TemperaturaCelsius DECIMAL(5,2),
    FrecuenciaRespiratoria INT,
    FrecuenciaCardiaca INT,
    Pulso INT,
    PresionArterial VARCHAR(20),
    SaturacionOxigeno DECIMAL(5,2),
    PesoKg DECIMAL(5,2),
    TallaCm DECIMAL(5,2),
    CircunferenciaAbdominalCm DECIMAL(5,2),
    
    TipoDiagnostico VARCHAR(20),
    DiagnosticoTexto VARCHAR(MAX),
    TipoConsulta VARCHAR(30),
    Recomendaciones VARCHAR(MAX)
);
GO

--Tabla de Examen Fisico
CREATE TABLE ExamenesFisicos (
    IdExamen INT IDENTITY(1,1) PRIMARY KEY,
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) ON DELETE CASCADE,
    Region VARCHAR(50) NOT NULL,
    EsNormal BIT NOT NULL,
    Detalle VARCHAR(MAX)
);
GO

--Tabla de Recetas
CREATE TABLE Recetas (
    IdReceta INT IDENTITY(1,1) PRIMARY KEY,
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) ON DELETE CASCADE,
    NombreMedicamento VARCHAR(150) NOT NULL,
    CantidadYDosis VARCHAR(150) NOT NULL
);
GO

--Tabla de Antecedentes Personales Patologicos
CREATE TABLE AntecedentesPersonales (
    IdAntecedente INT IDENTITY(1,1) PRIMARY KEY,
    NumeroExpediente VARCHAR(50) FOREIGN KEY REFERENCES Pacientes(NumeroExpediente),
    Enfermedad VARCHAR(150) NOT NULL,
    AnioInicio INT,
    TipoMedicacion VARCHAR(150)
);
GO

--Detalle para Paciente Embarazada (Ampliando campos del formulario)
CREATE TABLE DetalleEmbarazada (
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) PRIMARY KEY,
    FormulaObstetrica VARCHAR(50),
    PesoAnteriorKg DECIMAL(5,2),
    EdadGestacionalTexto VARCHAR(50),
    FUR DATE,
    FPP DATE,
    GrupoSanguineo VARCHAR(10),
    VIH VARCHAR(MAX),
    Sifilis VARCHAR(MAX),
    UltrasonografiaObs VARCHAR(MAX)
);
GO

--Detalle para Paciente Infantil (Basado en la sección de micronutrientes y protección social)
CREATE TABLE DetalleInfantil (
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) PRIMARY KEY,
    VitaminaASuministrada BIT,
    MultivitaminasSuministradas BIT,
    HierroSuministrado BIT,
    MebendazolSuministrado BIT,
    AlbendazolSuministrado BIT,
    EsquemaVacunacionCompleto BIT,
    SufreMaltratoViolencia BIT,
    CumpleDerechosNino BIT,
    PresentaRiesgoSocial BIT
);
GO

--Detalle para Adulto Mayor
CREATE TABLE DetalleAdultoMayor (
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) PRIMARY KEY,
    PuntajeBarthel INT,
    CategoriaDependencia VARCHAR(50)
);
GO

--Detalle para Paciente Crónico (Opcional por si requiere datos únicos)
CREATE TABLE DetalleCronico (
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) PRIMARY KEY,
    ComplicacionesRecurrentes VARCHAR(MAX),
    ApegadoTratamiento BIT
);
GO

--Detalle para Paciente Adolescente (Opcional)
CREATE TABLE DetalleAdolescente (
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) PRIMARY KEY,
    SaludSexualReproductiva VARCHAR(MAX),
    EvaluacionPsicosocial VARCHAR(MAX)
);
GO

--Detalle para Mujer en Edad Fértil (MEF)
CREATE TABLE DetalleMujerEdadFertil (
    IdConsulta INT FOREIGN KEY REFERENCES Consultas(IdConsulta) PRIMARY KEY,
    PlanificacionFamiliar BIT,
    MetodoAnticonceptivoActual VARCHAR(100)
);
GO

--Agregar o actualizar tabla de Usuarios
IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL 
    DROP TABLE dbo.Usuarios;
GO

CREATE TABLE Usuarios (
    idUsuario INT PRIMARY KEY IDENTITY(1,1),
    NombreUs VARCHAR(50) NOT NULL UNIQUE,
    contra VARCHAR(50) NOT NULL,
    Rol VARCHAR(20) NOT NULL
);
GO

--Insertar Usuarios de Prueba
INSERT INTO Usuarios (NombreUs, contra, Rol) 
VALUES 
    ('Carlos', 'Admin123', 'Administrador'),
    ('Sofi', 'Doc123', 'Médico');
GO

-- Agregar columna 'Activo' si no existe
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Usuarios' AND COLUMN_NAME = 'Activo'
)
BEGIN
    ALTER TABLE Usuarios ADD Activo BIT NOT NULL DEFAULT 1;
END;
GO

-- Actualizar usuarios existentes para que estén activos
UPDATE Usuarios SET Activo = 1 WHERE Activo IS NULL;
GO