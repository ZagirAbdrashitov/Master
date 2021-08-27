<template>
    <h1>Data mamagement</h1>

    <div id="outer">
        <div class="inner">
            <p>Save all data to file</p>
            <button class="btn btn-primary" @click="exportData">Export</button>
        </div>
        <div class="inner">
            <p>Load data from file</p>
            <button class="btn btn-primary" @click="importData">Import</button>
        </div>
    </div>

</template>

<script>
    export default {
        name: "Data",
        methods: {
            exportData() {

            },
            importData() {

            }
        }
    }
</script>