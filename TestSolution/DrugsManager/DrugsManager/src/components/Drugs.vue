<template>
    <h1 id="tableLabel">List of drugs</h1>

    <p v-if="!drugs"><em>Loading...</em></p>

    <table class='table table-striped' aria-labelledby="tableLabel" v-if="drugs">
        <thead>
            <tr>
                <th>NDC</th>
                <th>Name</th>
                <th>Pack size</th>
                <th>Unit</th>
                <th>Price ($)</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="drug of drugs" v-bind:key="drug">
                <td>{{ drug.ndc }}</td>
                <td>{{ drug.name }}</td>
                <td>{{ drug.packSize }}</td>
                <td>{{ drug.unit }}</td>
                <td>{{ drug.price }}</td>
            </tr>
        </tbody>
    </table>
</template>


<script>
    export default {
        name: "Drugs",
        data() {
            return {
                drugs: []
            }
        },
        methods: {
             getDrugs() {
                /*axios.get('/weatherforecast')
                    .then((response) => {
                        this.forecasts =  response.data;
                    })
                    .catch(function (error) {
                        alert(error);
                    });*/
            }
        },
        mounted() {
            this.getDrugs();
        }
    }
</script>