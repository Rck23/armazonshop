// DEFINIR LAS URL 

const prod = {
    url: {
        API_URL: 'Servidor de produccion'
    }
}

const dev = {
    url: {
        API_URL: 'dev'
    }
}

export const config = process.env.NODE_ENV == 'Development' ? dev : prod; 