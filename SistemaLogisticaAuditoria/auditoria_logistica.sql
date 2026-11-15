-- Ejecutado: 2026-11-14 19:57:18
CREATE TABLE IF NOT EXISTS TransaccionesEnviosRecepciones (
    IdTransaccion INTEGER PRIMARY KEY AUTOINCREMENT,
    CodigoPaquete TEXT NOT NULL,
    Cliente TEXT NOT NULL,
    Zona TEXT NOT NULL,
    PesoKg REAL NOT NULL,
    ValorDeclarado REAL NOT NULL,
    SubtotalPeso REAL NOT NULL,
    RecargoZona REAL NOT NULL,
    PenalizacionSobrepeso REAL NOT NULL,
    SeguroValorDeclarado REAL NOT NULL,
    CostoFijoEnvio REAL NOT NULL,
    SubtotalNeto REAL NOT NULL,
    MontoIVA REAL NOT NULL,
    TotalPagar REAL NOT NULL,
    EstadoTransaccion TEXT NOT NULL,
    FechaHoraRegistro TEXT NOT NULL
);

-- Ejecutado: 2026-11-14 19:57:18
BEGIN TRANSACTION;
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-004', 'Distribuidora Del Centro', 'ZONA C', 32.50, 3400.00, 812.50, 146.25, 78.12, 68.00, 60.00, 1164.87, 186.38, 1351.25, 'ENVIADO', '2026-11-14 19:57:18');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-006', 'Importaciones Bajio', 'ZONA C', 28.00, 2100.00, 700.00, 126.00, 50.00, 42.00, 60.00, 978.00, 156.48, 1134.48, 'ENVIADO', '2026-11-14 19:57:18');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-001', 'Ana Sofia Martinez', 'ZONA B', 24.00, 1500.00, 600.00, 60.00, 25.00, 30.00, 60.00, 775.00, 124.00, 899.00, 'ENVIADO', '2026-11-14 19:57:18');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-008', 'Farmacia San Marcos', 'ZONA A', 22.00, 950.00, 550.00, 27.50, 12.50, 0.00, 60.00, 650.00, 104.00, 754.00, 'ENVIADO', '2026-11-14 19:57:18');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-007', 'Ferreteria Aguascalientes', 'ZONA B', 18.00, 1250.00, 450.00, 45.00, 0.00, 25.00, 60.00, 580.00, 92.80, 672.80, 'ENVIADO', '2026-11-14 19:57:18');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('COD-101', 'Cliente Juan Perez', 'ZONA A', 15.50, 200.00, 387.50, 19.38, 0.00, 0.00, 60.00, 466.88, 74.70, 541.58, 'ENVIADO', '2026-11-14 19:57:18');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-005', 'Carlos Eduardo Ruiz', 'ZONA A', 8.00, 500.00, 200.00, 10.00, 0.00, 0.00, 60.00, 270.00, 43.20, 313.20, 'ENVIADO', '2026-11-14 19:57:18');
COMMIT;

-- Ejecutado: 2026-11-14 19:57:18
UPDATE TransaccionesEnviosRecepciones SET EstadoTransaccion = 'RECIBIDO EN DESTINO', FechaHoraRegistro = '2026-11-14 19:57:18' WHERE CodigoPaquete = 'PACK-004';

-- Ejecutado: 2026-11-14 19:58:56
CREATE TABLE IF NOT EXISTS TransaccionesEnviosRecepciones (
    IdTransaccion INTEGER PRIMARY KEY AUTOINCREMENT,
    CodigoPaquete TEXT NOT NULL,
    Cliente TEXT NOT NULL,
    Zona TEXT NOT NULL,
    PesoKg REAL NOT NULL,
    ValorDeclarado REAL NOT NULL,
    SubtotalPeso REAL NOT NULL,
    RecargoZona REAL NOT NULL,
    PenalizacionSobrepeso REAL NOT NULL,
    SeguroValorDeclarado REAL NOT NULL,
    CostoFijoEnvio REAL NOT NULL,
    SubtotalNeto REAL NOT NULL,
    MontoIVA REAL NOT NULL,
    TotalPagar REAL NOT NULL,
    EstadoTransaccion TEXT NOT NULL,
    FechaHoraRegistro TEXT NOT NULL
);

-- Ejecutado: 2026-11-14 19:58:56
BEGIN TRANSACTION;
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-004', 'Distribuidora Del Centro', 'ZONA C', 32.50, 3400.00, 812.50, 146.25, 78.12, 68.00, 60.00, 1164.87, 186.38, 1351.25, 'ENVIADO', '2026-11-14 19:58:56');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-006', 'Importaciones Bajio', 'ZONA C', 28.00, 2100.00, 700.00, 126.00, 50.00, 42.00, 60.00, 978.00, 156.48, 1134.48, 'ENVIADO', '2026-11-14 19:58:56');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-001', 'Ana Sofia Martinez', 'ZONA B', 24.00, 1500.00, 600.00, 60.00, 25.00, 30.00, 60.00, 775.00, 124.00, 899.00, 'ENVIADO', '2026-11-14 19:58:56');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-008', 'Farmacia San Marcos', 'ZONA A', 22.00, 950.00, 550.00, 27.50, 12.50, 0.00, 60.00, 650.00, 104.00, 754.00, 'ENVIADO', '2026-11-14 19:58:56');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-007', 'Ferreteria Aguascalientes', 'ZONA B', 18.00, 1250.00, 450.00, 45.00, 0.00, 25.00, 60.00, 580.00, 92.80, 672.80, 'ENVIADO', '2026-11-14 19:58:56');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('COD-101', 'Cliente Juan Perez', 'ZONA A', 15.50, 200.00, 387.50, 19.38, 0.00, 0.00, 60.00, 466.88, 74.70, 541.58, 'ENVIADO', '2026-11-14 19:58:56');
INSERT INTO TransaccionesEnviosRecepciones (CodigoPaquete, Cliente, Zona, PesoKg, ValorDeclarado, SubtotalPeso, RecargoZona, PenalizacionSobrepeso, SeguroValorDeclarado, CostoFijoEnvio, SubtotalNeto, MontoIVA, TotalPagar, EstadoTransaccion, FechaHoraRegistro) VALUES ('PACK-005', 'Carlos Eduardo Ruiz', 'ZONA A', 8.00, 500.00, 200.00, 10.00, 0.00, 0.00, 60.00, 270.00, 43.20, 313.20, 'ENVIADO', '2026-11-14 19:58:56');
COMMIT;

-- Ejecutado: 2026-11-14 19:58:56
UPDATE TransaccionesEnviosRecepciones SET EstadoTransaccion = 'RECIBIDO EN DESTINO', FechaHoraRegistro = '2026-11-14 19:58:56' WHERE CodigoPaquete = 'PACK-004';

