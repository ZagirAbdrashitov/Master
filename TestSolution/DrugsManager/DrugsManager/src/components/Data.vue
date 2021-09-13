<template>
    <h1>Data mamagement</h1>

    <div id="outer">
        <div class="inner">
            <p>Save all data to file</p>
            <button class="btn btn-primary" @click="exportData">Export</button>
        </div>
        <div class="inner">
            <p>Load data from file</p>
            <input type="file" ref="importFile" style="display: none" accept=".csv" v-on:change="handleFileUpload">
            <button class="btn btn-primary" @click="importData">Import</button>
        </div>
    </div>

</template>

<script>
    import axios from "axios";

    export default {
        name: "Data",
        data() {
            return {
                fileData: ''
            }
        },
        methods: {
            exportData() {
                axios({
                    method: 'get',
                    url: 'api/Drugs/export',
                    responseType: 'blob'
                    })
                    .then((response) => {
                        var currentdate = new Date();
                        var datetime = currentdate.today() + "-" + currentdate.timeNow();
                        const blob = new Blob([response.data]);
                        const link = document.createElement('a');
                        link.href = URL.createObjectURL(blob);
                        link.download = "Drugs_list_" + datetime + ".csv";
                        link.click();
                        URL.revokeObjectURL(link.href);
                    })
                    .catch(function (error) {
                        alert(error);
                    });

                Date.prototype.today = function () {
                    return ((this.getDate() < 10) ? "0" : "") + this.getDate() + "." +
                        (((this.getMonth() + 1) < 10) ? "0" : "") + (this.getMonth() + 1) + "." +
                        this.getFullYear();
                }

                Date.prototype.timeNow = function () {
                    return ((this.getHours() < 10) ? "0" : "") + this.getHours() + "." +
                        ((this.getMinutes() < 10) ? "0" : "") + this.getMinutes() + "." +
                        ((this.getSeconds() < 10) ? "0" : "") + this.getSeconds();
                }
            },
            importData() {
                let fileInput = this.$refs.importFile;
                fileInput.click();
            },
            handleFileUpload() {
                this.fileData = this.$refs.importFile.files[0];

                let formData = new FormData();
                formData.append('file', this.fileData);
                axios.post("api/Drugs/import",
                    formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    })
                    .then((response) => {
                        alert(response.data);
                    })
                    .catch(function (error) {
                        alert(error.response.data);
                    });
            }
        }
    }
</script>