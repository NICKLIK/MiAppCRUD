import React, { useState, useEffect } from 'react';
import './Productos.css';  // Agrega el archivo CSS para los estilos

function Productos() {
    const [productos, setProductos] = useState([]);

    useEffect(() => {
        // Hacer la solicitud GET a la API de productos
        fetch('https://localhost:52291/api/producto')
            .then((response) => response.json())
            .then((data) => setProductos(data))
            .catch((error) => console.error('Error al cargar productos:', error));
    }, []);

    return (
        <div className="productos-container">
            <h1>Lista de Productos</h1>
            <div className="productos-list">
                {productos.map((producto) => (
                    <div className="producto-card" key={producto.id}>
                        <h3 className="producto-name">{producto.nombre}</h3>
                        <p className="producto-description">{producto.descripcion}</p>
                        <p className="producto-price">${producto.precio}</p>
                        <p className="producto-stock">Stock: {producto.stock}</p>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Productos;
