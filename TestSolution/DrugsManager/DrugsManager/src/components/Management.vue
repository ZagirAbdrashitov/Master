<template>
    <h1>Drugs mamagement</h1>

    <p>Add new drugs or edit existing.</p>

    <table class='table table-striped'>
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
            <tr>
                <td>
                    <input type="text" v-model="ndcSearch" />
                </td>
                <td>
                    <input type="text" v-model="name" />
                </td>
                <td>
                    <input type="text" v-model="packSize" />
                </td>
                <td>
                    <select v-model="unit">
                        <option value="0">Small Pack</option>
                        <option value="1">Medium Pack</option>
                        <option value="2">Large Pack</option>
                    </select>
                </td>
                <td>
                    <input type="text" v-model="price" />
                </td>
            </tr>
        </tbody>
    </table>

    <div id="outer">
        <div class="inner">
            <button class="btn btn-primary" @click="check">Check</button>
        </div>
        <div class="inner">
            <button class="btn btn-primary" @click="submit">Submit</button>
        </div>
    </div>

</template>

<script>
    export default {
        name: "Management",
        data() {
            return {
                ndcSearch: '',
                name: '',
                packSize: 0,
                unit: 0,
                price: 0
            }
        },
        methods: {
            validateNdc() {
                if (this.ndcSearch == '') {
                    alert("NDC is empty");
                    return false;
                }
                if (this.ndcSearch.length != 8) {
                    alert("Wrong NDC format");
                    return false;
                }
                return true;
            },
            check() {
                if (!this.validateNdc()) {
                    return
                }
                alert("Drug with NDC [" + this.ndcSearch + "] not found");
            },
            submit() {
                if (!this.validateNdc()) {
                    return
                }
                alert("Not all parameters are filled");
            }
        }
    }
</script>