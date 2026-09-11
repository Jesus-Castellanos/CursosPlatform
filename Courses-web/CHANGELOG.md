En este archivo se explica cómo Visual Studio creado el proyecto.

Se usaron las siguientes herramientas para generar este proyecto:
- Angular CLI (ng)

Los pasos siguientes se usaron para generar este proyecto:
- Cree un proyecto de Angular con ng: `ng new "Courses-web" --defaults --skip-install --skip-git --no-standalone `.
- Actualizar angular.json con puerto.
- Crear archivo de proyecto (`Courses-web.esproj`).
- Crear `launch.json` para habilitar la depuración.
- Actualice package.json para agregar `jest-editor-support`.
- Actualice `start` el script en `package.json` para especificar el host.
- Agregar `karma.conf.js` para pruebas unitarias.
- Actualizar `angular.json` para que apunte a `karma.conf.js`.
- Agregue el proyecto a la solución.
- Escriba este archivo.
