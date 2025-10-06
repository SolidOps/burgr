# burgr
A source generator using templates and DDD.

# YAML description

* aggregate_roots / entities / transients / value_objects / events
  * namespace
  * interface
  * id_column_name
  * table_name
  * forced_prefix
  * extends
  * event_produced
    * add
    * update
    * remove
  * id_id_private
  * cacheable
  * identity_keys_type
  * properties
    * type
      * $: enum
      * []: array
      * ?: nullable
      * identity / void / text
      * ->: navigation
      * =: calculated
      * -: non persisted
    * max_size
    * field_size
    * is_unique
    * is_unique_case_sensitive
    * is_label
    * is_queryable
    * default_value
    * special_type
    * back_link_navigation
    * join_navigation
    * db_column_name
    * is_private
    * id_column_name
    * multiple_unique_constraint_with
  * api (query, get, add, update, remove)
    * is_anonymous
    * mandatory_right
    * ownership_override_right
  * event_consumers
    * name of events
  * dependencies
    * names of other entities
  * components
    * list
    * details
  * views
    * list
    * details
  * rules
    * before_add
    * after_add
    * before_update
    * after_update
    * before_remove
    * after_remove
  * data_type (only_events)

* use_cases
  * namespace
  * is_anonymous
  * create_rest_services
  * mandatory_right
  * ownership_override_right
  * is_internal
  * implements_interfaces
  * steps
    * inputs
    * result
    * no_transaction
    * mandatory_right
    * ownership_override_roght
    * force_post
    * create_component
* enums
  * namespace
  * values
