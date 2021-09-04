<template>
    <h1 id="tableLabel">List of drugs</h1>

    <p v-if="!form.drugs"><em>Loading...</em></p>

    <button type="button" class="btn btn-primary m-2 fload-end" @click="addClick()" data-toggle="modal" data-target="#drugModal">
        Add Drug
    </button>

    <table class='table table-striped' aria-labelledby="tableLabel" v-if="form.drugs">
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
            <tr v-for="drug of form.drugs" v-bind:key="drug">
                <td>{{ drug.Ndc }}</td>
                <td>{{ drug.Name }}</td>
                <td>{{ drug.PackSize }}</td>
                <td>{{ form.units[drug.Unit] }}</td>
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
                        <h5 class="modal-title" id="drugModalLabel">{{ form.modalTitle }}</h5>
                        <button type="button" class="btn btn-light mr-1" data-dismiss="modal" aria-label="Close">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-square-fill" viewBox="0 0 16 16">
                                <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2zm3.354 4.646L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 1 1 .708-.708z" />
                            </svg>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="d-flex flex-row bd-highlight mb-3">
                            <div class="p-2 w-100 bd-highlight">
                                <div class="error" style="color:red" v-if="v$.form.ndc.$error">
                                    <template v-for="error of v$.form.ndc.$errors" :key="error.$uid">
                                        {{ error.$message }} <br>
                                    </template>
                                </div>
                                <div class="input-group mb-3">
                                    <span class="input-group-text">NDC</span>
                                    <input type="text" class="form-control" v-model="form.ndc" @blur="v$.form.ndc.$touch()">
                                </div>
                                <div class="error" style="color:red" v-if="v$.form.name.$error">
                                    <template v-for="error of v$.form.name.$errors" :key="error.$uid">
                                        {{ error.$message }} <br>
                                    </template>
                                </div>
                                <div class="input-group mb-3">
                                    <span class="input-group-text">Name</span>
                                    <input type="text" class="form-control" v-model="form.name" @blur="v$.form.name.$touch()">
                                </div>
                                <div class="error" style="color:red" v-if="v$.form.packSize.$error">
                                    <template v-for="error of v$.form.packSize.$errors" :key="error.$uid">
                                        {{ error.$message }} <br>
                                    </template>
                                </div>
                                <div class="input-group mb-3">
                                    <span class="input-group-text">Pack size</span>
                                    <input type="text" class="form-control" v-model="form.packSize" @blur="v$.form.packSize.$touch()">
                                </div>
                                <div class="input-group mb-3">
                                    <span class="input-group-text">Unit</span>
                                    <select class="form-select" name="unitSelect" v-model="form.unit">
                                        <option v-for="(item, index) in form.units" :key="index" :value="{ id: index, text: item }">
                                            {{ item }}
                                        </option>
                                    </select>
                                </div>
                                <div class="error" style="color:red" v-if="v$.form.price.$error">
                                    <template v-for="error of v$.form.price.$errors" :key="error.$uid">
                                        {{ error.$message }} <br>
                                    </template>
                                </div>
                                <div class="input-group mb-3">
                                    <span class="input-group-text">Price ($)</span>
                                    <input type="text" class="form-control" v-model="form.price" @blur="v$.form.price.$touch()">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="submit" @click="createClick()" v-if="!form.id" class="btn btn-primary" data-dismiss="modal" :disabled="v$.form.$invalid">
                            Create
                        </button>
                        <button type="submit" @click="updateClick()" v-if="form.id" class="btn btn-primary" data-dismiss="modal" :disabled="v$.form.$invalid">
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
    import useVuelidate from '@vuelidate/core';
    import { helpers, required, minValue, maxValue, maxLength, minLength, alphaNum, integer } from '@vuelidate/validators';

    const twoDecimalPlaces = (value) => value != null && /^[-]?\d*(\.\d+)?$/.test(value) && value.indexOf(".") > -1 && (value.split('.')[1].length == 2);

    export default {
        name: "Drugs",
        setup: () => ({ v$: useVuelidate() }),
        data() {
            return {
                form: {
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
            }
        },
        validations: {
            form: {
                ndc: {
                    required,
                    alphaNum,
                    minLength: minLength(8),
                    maxLength: maxLength(8),
                    isUnique(value) {
                        if (value === '') return true;
                        if (this.form.drugs) return !this.form.drugs.find(item => item.Ndc == value);
                        return true;
                    }
                },
                name: {
                    required,
                    minLength: minLength(3),
                    maxLength: maxLength(255)
                },
                packSize: {
                    required,
                    integer,
                    minValue: minValue(1),
                    maxValue: maxValue(2147483647)
                },
                price: {
                    required,
                    minValue: minValue(0.01),
                    maxValue: maxValue(99999999999999999.99),
                    twoDecimalPlaces: helpers.withMessage('Value must be number with two decimals', twoDecimalPlaces)
                }
            }
        },
        methods: {
            submitAction() {
                alert('Form submitted');
            },
            refreshData() {
                axios.get("api/Drugs")
                    .then((response) => {
                        this.form.drugs = response.data;
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            addClick() {
                this.form.modalTitle = "Add Drug";
                this.form.id = null;
                this.form.ndc = "";
                this.form.name = "";
                this.form.packSize = 0;
                this.form.unit.id = 0;
                this.form.unit.text = this.form.units[0];
                this.form.price = 0;
            },
            editClick(drug) {
                this.form.modalTitle = "Edit Drug";
                this.form.id = drug.Id;
                this.form.ndc = drug.Ndc;
                this.form.name = drug.Name;
                this.form.packSize = drug.PackSize;
                this.form.unit.id = drug.Unit;
                this.form.unit.text = this.form.units[drug.Unit];
                this.form.price = drug.Price;
            },
            createClick() {
                axios.post("api/Drugs", {
                    Ndc: this.form.ndc,
                    Name: this.form.name,
                    PackSize: this.form.packSize,
                    Unit: this.form.unit.id,
                    Price: this.form.price
                })
                    .then((response) => {
                        this.form.drugs.push({
                            Id: response.data,
                            Ndc: this.form.ndc,
                            Name: this.form.name,
                            Unit: this.form.unit.id,
                            PackSize: this.form.packSize,
                            Price: this.form.price
                        });
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            },
            updateClick() {
                axios.put("api/Drugs", {
                    Id: this.form.id,
                    Ndc: this.form.ndc,
                    Name: this.form.name,
                    PackSize: this.form.packSize,
                    Unit: this.form.unit.id,
                    Price: this.form.price
                })
                    .then(() => {
                        let index = this.form.drugs.findIndex(r => r.Id == this.form.id);
                        this.form.drugs[index].Ndc = this.form.ndc;
                        this.form.drugs[index].Name = this.form.name;
                        this.form.drugs[index].PackSize = this.form.packSize;
                        this.form.drugs[index].Unit = this.form.unit.id;
                        this.form.drugs[index].Price = this.form.price;
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
                        let index = this.form.drugs.findIndex(r => r.Id == id);
                        this.form.drugs.splice(index, 1);
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