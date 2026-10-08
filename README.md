# FinTrack

API REST para el seguimiento de gastos personales y compartidos, hecha con .NET y EF Core.

**Stack:** C#, ASP.NET Core, EF Core, MediatR, FluentValidation, xUnit, NSubstitute.
**Arquitectura:** Clean Architecture, DDD y CQRS, con los patrones Repository, Unit of Work y Result.

## Funcionalidades

- Inicio de sesión en dos etapas (cuenta y perfil), con un token para cada una.
- Protección opcional del perfil con contraseña; el perfil administrador puede desactivarla.
- Gastos personales y compartidos, con asignación del monto que corresponde a cada perfil.

## Limitaciones conocidas y próximos pasos

- Los listados de gastos aún no están paginados.
- La validación de pertenencia del recurso en los handlers está en revisión.
- Hay pruebas unitarias de parte del dominio; faltan más entidades, handlers y pruebas de integración.
- Balance por perfil (ingresos menos lo pagado): pendiente.

> README en construcción: próximamente instrucciones de ejecución y lista de endpoints.