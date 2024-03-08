#!/usr/bin/env python3


from flask import Flask, jsonify
import pandas as pd

app = Flask(__name__)

@app.route('/data', methods=['GET'])
def get_data():
    try:
        # Lee el archivo Excel
        df = pd.read_excel('db.xlsx')
        
        # Convierte los datos a un formato JSON
        data = df.to_dict(orient='records')
        
        return jsonify(data), 200
    except Exception as e:
        return jsonify({'error': str(e)}), 500

if __name__ == '__main__':
    app.run(debug=True)
