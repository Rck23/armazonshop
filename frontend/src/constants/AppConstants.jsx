// DEFINIR LAS URL 

const prod = {
    url: {
        API_URL: 'http://localhost:5000'
    }
}

const dev = {
    url: {
        API_URL: 'http://localhost:5000'
    }
}

export const config = process.env.NODE_ENV === 'Development' ? dev : prod; 