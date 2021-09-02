<template>
    <h1 id="tableLabel">List of drugs</h1>

    <p v-if="!drugs"><em>Loading...</em></p>

    <button type="button" class="btn btn-primary m-2 fload-end" @click="addClick()" data-toggle="modal" data-target="#drugModal">
        Add Drug
    </button>

    <table class='table table-striped' aria-labelledby="tableLabel" v-if="drugs">
        <thead>
            <tr>
                <th>NDC</th>
                <th>Name</th>
                <th>Pack size</th>
                <th>Unit</th>
                <th>Price ($)</th>
                <th>Options</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="drug of drugs" v-bind:key="drug">
                <td>{{ drug.Ndc }}</td>
                <td>{{ drug.Name }}</td>
                <td>{{ drug.PackSize }}</td>
                <td>{{ units[drug.Unit] }}</td>
                <td>{{ drug.Price }}</td>
                <td>
                    <button type="button" class="btn btn-light mr-1" @click="editClick(drug)" data-toggle="modal" data-target="#drugModal">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
                            <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                            <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />
                        </svg>
                    </button>
                    <button type="button" @click="deleteClick(drug.Id)" class="btn btn-light mr-1">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">
                            <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                        </svg>
                    </button>
                </td>
            </tr>
        </tbody>
    </table>

    <form @submit.prevent="submitAction()">
        <div class="modal fade" id="drugModal" tabindex="-1" aria-labelledby="drugModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="drugModalLabel">{{ modalTitle }}</h5>
                        <button type="button" class="btn btn-light mr-1" data-dismiss="modal" aria-label="Close">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-square-fill" viewBox="0 0 16 16">
                                <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2zm3.354 4.646L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 1 1 .708-.708z" />
                            </svg>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="d-flex flex-row bd-highlight mb-3">
                            <div class="p-2 w-50 bd-highlight">
                                <div class="input-group mb-3">
                                    <span class="input-group-text">NDC</span>
                                    <input type="text" class="form-control" v-model="$v.ndc.$model" @blur="$v.ndc.$touch()">
                                    <div class="error" v-if="$v.ndc.$error">
                                        <template v-if="!$v.ndc.validFormat">
                                            National drug code must contain only letters or digits 8 characters length
                                        </template>
                                        <template v-else-if="!$v.ndc.isUnique">
                                            Drug with this NDC already exists
                                        </template>
                                        <template v-else>
                                            Field is required
                                        </template>
                                    </div>
                                </div>
                                <div class="input-group mb-3">
                                    <span class="input-group-text">Name</span>
                                    <input type="text" class="form-control" v-model="$v.name.$model" @blur="$v.name.$touch()">
                                    <div class="error" v-if="$v.name.$error">
                                        <template v-if="!$v.name.maxLength">
                                            Name is too long
                                        </template>
                                        <template v-else>
                                            Field is required
                                        </template>
                                    </div>
                                </div>
                                <div class="input-group mb-3">
                                    <span class="input-group-text">Pack size</span>
                                    <input type="text" class="form-control" v-model="$v.packSize.$model" @blur="$v.packSize.$touch()">
                                    <div class="error" v-if="$v.packSize.$error">
                                        <template v-if="!$v.packSize.numeric">
                                            Numeric format required
                                        </template>
                                        <template v-else>
                                            Field is required
                                        </template>
                                    </div>
                                </div>
                                <div class="input-group mb-3">
                                    <span class="input-group-text">Unit</span>
                                    <select class="form-select" name="unitSelect" v-model="unit">
                                        <option v-for="(item, index) in units" :key="index" :value="{ id: index, text: item }">
                                            {{ item }}
                                        </option>
                                    </select>
                                </div>
                                <div class="input-group mb-3">
                                    <span class="input-group-text">Price</span>
                                    <input type="text" class="form-control" v-model="price">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="submit" @click="createClick()" v-if="!id" class="btn btn-primary" data-dismiss="modal" :disabled="$v.validationGroup.$invalid">
                            Create
                        </button>
                        <button type="submit" @click="updateClick()" v-if="id" class="btn btn-primary" data-dismiss="modal" :disabled="$v.validationGroup.$invalid">
                            Update
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </form>
</template>

<script>
    import axios from "axios";
    import { validationMixin } from "vuelidate";
    import { required, maxLength, numeric } from "@vuelidate/validators";

    export default {
        name: "Drugs",
        mixins: [validationMixin],
        data() {
            return {
                units: ['Small Pack', 'Medium Pack', 'Large Pack'],
                drugs: [],
                modalTitle: "",
                id: null,
                ndc: "",
                name: "",
                packSize: 0,
                unit: { id: 0, text: '' },
                price: 0
            }
        },
        validations() {
            return {
                ndc: {
                    required,
                    validFormat: val => /^\w{8}$/.test(val),
                    isUnique: this.isNdcUnique
                },
                name: {
                    required,
                    maxLength: maxLength(500)
                },
                packSize: {
                    required,
                    numeric
                },
                validationGroup: ['ndc', 'name', 'packSize']
            }
        },
        methods: {
            submitAction() {
                alert('Form submitted');
            },
            isNdcUnique = (value, vm) => {
                if (value === '') return true;
                if (vm.drugs) return !vm.drugs.find(item => item.Ndc == value);
                return true;
            },
            refreshData() {
                axios.get("api/Drugs")
                    .then((response) => {
                        this.drugs = response.data;
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            addClick() {
                this.modalTitle = "Add Drug";
                this.id = null;
                this.ndc = "";
                this.name = "";
                this.packSize = 0;
                this.unit.id = 0;
                this.unit.text = this.units[0];
                this.price = 0;
            },
            editClick(drug) {
                this.modalTitle = "Edit Drug";
                this.id = drug.Id;
                this.ndc = drug.Ndc;
                this.name = drug.Name;
                this.packSize = drug.PackSize;
                this.unit.id = drug.Unit;
                this.unit.text = this.units[drug.Unit];
                this.price = drug.Price;
            },
            createClick() {
                axios.post("api/Drugs", {
                    Ndc: this.ndc,
                    Name: this.name,
                    PackSize: this.packSize,
                    Unit: this.unit.id,
                    Price: this.price
                })
                    .then((response) => {
                        this.drugs.push({
                            Id: response.data,
                            Ndc: this.ndc,
                            Name: this.name,
                            Unit: this.unit.id,
                            PackSize: this.packSize,
                            Price: this.price
                        });
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            updateClick() {
                axios.put("api/Drugs", {
                    Id: this.id,
                    Ndc: this.ndc,
                    Name: this.name,
                    PackSize: this.packSize,
                    Unit: this.unit.id,
                    Price: this.price
                })
                    .then(() => {
                        let index = this.drugs.findIndex(r => r.Id == this.id);
                        this.drugs[index].Ndc = this.ndc;
                        this.drugs[index].Name = this.name;
                        this.drugs[index].PackSize = this.packSize;
                        this.drugs[index].Unit = this.unit.id;
                        this.drugs[index].Price = this.price;
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            deleteClick(id) {
                if (!confirm("Are you sure?")) {
                    return;
                }
                axios.delete("api/Drugs/" + id)
                    .then(() => {
                        let index = this.drugs.findIndex(r => r.Id == id);
                        this.drugs.splice(index, 1);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            }
        },
        mounted() {
            this.refreshData();
        }
    }
</script>